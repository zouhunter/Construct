using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public enum BuildState
{
    normal,
    inbuild
}
public class BuildingItem : MonoBehaviour, ISelectable
{
    /// <summary>
    /// 方块信息
    /// </summary>
    public struct Square
    {
        public Vector3 bottomLeft;
        public Vector3 bottomRight;
        public Vector3 topLeft;
        public Vector3 topRight;

        public Vector3 this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.bottomLeft;
                    case 1:
                        return this.bottomRight;
                    case 2:
                        return this.topLeft;
                    case 3:
                        return this.topRight;
                    case 4:
                        return this.bottomLeft; //so we wrap around back to start
                    default:
                        return Vector3.zero;
                }
            }
        }
    }
    public string deviceName;
    public QuadInfo quadInfo;
    [SerializeField]
    private Transform viewPos;
    private BoxCollider _boxCollider;
    private RaycastHit[] hits;
    private Vector3 colliderScale;
    public UnityAction<Vector3> onPositionChanged;
    public BuildState buildState = BuildState.normal;

    public Transform TransformComponent
    {
        get
        {
            return transform;
        }
    }

    BuildState ISelectable.BuildState
    {
        get
        {
            return buildState;
        }

        set
        {
            buildState = value;
        }
    }

    private void OnEnable()
    {
        quadInfo = new global::QuadInfo();
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void UpdateBuilding(Vector3 newPos)
    {
        buildState = BuildState.inbuild;
        UpdateQuad();
        quadInfo.installAble = JudePositionByOtherItem(newPos);

        transform.position = newPos;
        if (onPositionChanged != null) onPositionChanged.Invoke(transform.position);
    }

    public bool ResetPosition(Vector3 newPos)
    {
        var canInstall = JudePositionByOtherItem(newPos) && JudePositionByPanel(newPos);

        if (canInstall)
        {
            transform.position = newPos;
        }
        return canInstall;
    }
    #region 观察点信息
    public void SetViewAngle(float angle)
    {
        var distence = Vector3.Distance(transform.position, viewPos.position);
        viewPos.position = Quaternion.Euler(0, angle, 0) * -transform.forward * distence + transform.position;
    }
    public void SetViewDistence(float distence)
    {
        var dir = viewPos.position - transform.position;
        viewPos.position = dir.normalized * distence + transform.position;
    }
    public Vector3 GetViewPos()
    {
        return viewPos.localPosition;
    }
    public void SetViewPos(Vector3 pos)
    {
        viewPos.localPosition = pos;
    }
    public void GetViewPos(out float angle,out float distence)
    {
        angle = Vector3.Angle(-transform.forward,viewPos.position - transform.position);
        distence = Vector3.Distance(transform.position,viewPos.position);
    }
    #endregion
    #region 参考点绘制数据
    private void UpdateTrangles()
    {
        var handleTriangles = new List<Vector3>();
        viewPos.transform.LookAt(transform);
        AddTriangles(viewPos.position, (transform.position - viewPos.position).normalized, Vector3.up, viewPos.right, 0.1f, handleTriangles);
        quadInfo.viewPosTrangles = handleTriangles.ToArray();
    }
    private void AddTriangles(Vector3 axisEnd, Vector3 axisDirection, Vector3 axisOtherDirection1, Vector3 axisOtherDirection2, float size, List<Vector3> resultsBuffer)
    {
        Vector3 endPoint = axisEnd + (axisDirection * (size * 2f));
        Square baseSquare = GetBaseSquare(axisEnd, axisOtherDirection1, axisOtherDirection2, size / 2f);

        resultsBuffer.Add(baseSquare.bottomLeft);
        resultsBuffer.Add(baseSquare.topLeft);
        resultsBuffer.Add(baseSquare.topRight);
        resultsBuffer.Add(baseSquare.topLeft);
        resultsBuffer.Add(baseSquare.bottomRight);
        resultsBuffer.Add(baseSquare.topRight);

        for (int i = 0; i < 4; i++)
        {
            resultsBuffer.Add(baseSquare[i]);
            resultsBuffer.Add(baseSquare[i + 1]);
            resultsBuffer.Add(endPoint);
        }
    }
    private Square GetBaseSquare(Vector3 axisEnd, Vector3 axisOtherDirection1, Vector3 axisOtherDirection2, float size)
    {
        Square square;
        Vector3 offsetUp = ((axisOtherDirection1 * size) + (axisOtherDirection2 * size));
        Vector3 offsetDown = ((axisOtherDirection1 * size) - (axisOtherDirection2 * size));
        //These arent really the proper directions, as in the bottomLeft isnt really at the bottom left...
        square.bottomLeft = axisEnd + offsetDown;
        square.topLeft = axisEnd + offsetUp;
        square.bottomRight = axisEnd - offsetDown;
        square.topRight = axisEnd - offsetUp;
        return square;
    }
    #endregion

    /// <summary>
    /// 更新框体绘制信息
    /// </summary>
    public void UpdateQuad()
    {
        UpdateTrangles();
        colliderScale = BuildingUtility.ScaleAndCubeScale(transform.localScale, _boxCollider.size);
        quadInfo.quad = UpdateQuad(transform.position, transform.rotation, colliderScale.x * 1.2f, colliderScale.z * 1.2f);
    }
    private static Vector3[] UpdateQuad(Vector3 center, Quaternion rot, float wigth, float length)
    {
        var quad = new Vector3[4];
        quad[0] = center + rot * new Vector3(-wigth * 0.5f, 0.01f, length * 0.5f);
        quad[1] = center + rot * new Vector3(-wigth * 0.5f, 0.01f, -length * 0.5f);
        quad[2] = center + rot * new Vector3(wigth * 0.5f, 0.01f, -length * 0.5f);
        quad[3] = center + rot * new Vector3(wigth * 0.5f, 0.01f, length * 0.5f);
        return quad;
    }
    /// <summary>
    /// 判断一点是否可以放置对象
    /// </summary>
    /// <param name="newPos"></param>
    /// <returns></returns>
    private bool JudePositionByOtherItem(Vector3 newPos)
    {
        hits = Physics.BoxCastAll(
            _boxCollider.center + newPos,
            colliderScale * 0.5f * 1.2f,
            Vector3.up,
            transform.rotation,
            10,
            LayerMask.GetMask(BuildingUtility.MoveItemLayerName));

        bool canInstall = true;

        foreach (var item in hits)
        {
            if (item.collider.gameObject != gameObject)
            {
                canInstall = false;
            }
        }

        return canInstall;
    }
    /// <summary>
    /// 如果通过界面来移动对象需要判断是否与可安装点接触
    /// </summary>
    /// <param name="newPos"></param>
    /// <returns></returns>
    private bool JudePositionByPanel(Vector3 newPos)
    {
        bool canInstall = true;
        hits = Physics.BoxCastAll(
       _boxCollider.center + newPos,
       Vector3.one * 0.01f,//中心点
       Vector3.down,
       transform.rotation,
       10,
       LayerMask.GetMask(BuildingUtility.MovePlaneLayerName));

        if (hits.Length == 0)
        {
            canInstall = false;
        }
        return canInstall;
    }
}

