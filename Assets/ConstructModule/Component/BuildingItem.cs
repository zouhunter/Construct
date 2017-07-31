using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public enum BuildState
{
    normal,
    inbuild
}
public class BuildingItem : MonoBehaviour
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

    public QuadInfo quadInfo;
    [SerializeField] private Transform viewPos;
    private BoxCollider _boxCollider;
    private RaycastHit[] hits;
    private Vector3 colliderScale;
    public UnityAction<QuadInfo> onPositionChanged;
    public BuildState buildState = BuildState.normal;
    private void OnEnable()
    {
        quadInfo = new global::QuadInfo();
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void UpdateBuilding(Vector3 newPos)
    {
        buildState = BuildState.inbuild;
        transform.position = newPos;
        UpdateQuad();
        JudePosition();
    }
    private void UpdateTrangles()
    {
        var handleTriangles = new List<Vector3>();
        viewPos.transform.LookAt(transform);
        AddTriangles(viewPos.position, (transform.position - viewPos.position).normalized, Vector3.up, viewPos.right, 0.1f, handleTriangles);
       quadInfo.viewPosTrangles = handleTriangles.ToArray();
    }

    void AddTriangles(Vector3 axisEnd, Vector3 axisDirection, Vector3 axisOtherDirection1, Vector3 axisOtherDirection2, float size, List<Vector3> resultsBuffer)
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
    Square GetBaseSquare(Vector3 axisEnd, Vector3 axisOtherDirection1, Vector3 axisOtherDirection2, float size)
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

    public void UpdateQuad()
    {
        UpdateTrangles();
        colliderScale = BuildingUtility.ScaleAndCubeScale(transform.localScale, _boxCollider.size);
        quadInfo.quad = UpdateQuad(transform.position,transform.rotation, colliderScale.x * 1.2f, colliderScale.z * 1.2f);
    }

    private static Vector3[] UpdateQuad(Vector3 center,Quaternion rot,float wigth,float length)
    {
        var quad = new Vector3[4];
        quad[0] = center + rot * new Vector3(-wigth * 0.5f,0.01f, length * 0.5f );
        quad[1] = center + rot * new Vector3(-wigth * 0.5f, 0.01f, -length * 0.5f );
        quad[2] = center + rot * new Vector3(wigth * 0.5f, 0.01f, -length * 0.5f);
        quad[3] = center + rot * new Vector3(wigth * 0.5f, 0.01f, length * 0.5f);
        return quad;
    }
    private void JudePosition()
    {
        hits = Physics.BoxCastAll(
            _boxCollider.center + transform.position,
            colliderScale * 0.5f * 1.2f,
            Vector3.up,
            transform.rotation,
            10,
            LayerMask.GetMask(BuildingUtility.MoveItemLayerName));

        quadInfo.installAble = true;

        foreach (var item in hits)
        {
            if (item.collider.gameObject != gameObject)
            {
                quadInfo.installAble = false;
            }
        }
    }
}

