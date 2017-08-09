using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;
public sealed class SelectDrawer : MonoBehaviour
{
    public Material lineMaterial { get; private set; }

    private Camera _camera;
    private Vector3 _startPos;
    private bool _needDraw;

    public event UnityAction<ISelectable[]> onGetRootObjs;
    private ISelectable hitTrans = null;

    private void Awake()
    {
        if (lineMaterial == null)
            lineMaterial = new Material(Shader.Find("Custom/Lines"));
        _camera = GetComponent<Camera>();
    }
    private void OnEnable()
    {
        _needDraw = false;
    }
    private void OnPostRender()
    {
        if (_needDraw)
        {
            DrawQuater();
        }
    }

    private void DrawQuater()
    {
        Vector3 endPos = Input.mousePosition;
        Vector3[] linePoints = new Vector3[4];
        linePoints[0] = _startPos;
        linePoints[1] = _startPos + Vector3.up * (endPos.y - _startPos.y);
        linePoints[2] = endPos;
        linePoints[3] = _startPos + Vector3.right * (endPos.x - _startPos.x);

        GL.PushMatrix();
        lineMaterial.SetPass(0);
        //坐标转化
        GL.LoadPixelMatrix();
        GL.Begin(GL.LINES);
        GL.Color(Color.green);

        for (int i = 0; i < linePoints.Length; ++i)
        {
            if (i + 1 == linePoints.Length)
            {
                GL.Vertex(linePoints[i]);
                GL.Vertex(linePoints[0]);
            }
            else
            {
                GL.Vertex(linePoints[i]);
                GL.Vertex(linePoints[i + 1]);
            }
        }
        GL.Vertex(linePoints[3]);
        GL.Vertex(linePoints[0]);

        GL.End();
        GL.PopMatrix();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                _needDraw = false;
                return;
            }

            RaycastHit hitInfo;
            var hit = Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hitInfo);

            if (hit && hitInfo.collider != null && hitInfo.collider.GetComponent(typeof(ISelectable)) != null)
            {
                hitTrans = hitInfo.collider.GetComponent(typeof(ISelectable)) as ISelectable;
            }
            else
            {
                hitTrans = null;
            }

            if (onGetRootObjs != null)
                onGetRootObjs(hitTrans == null ? null : new ISelectable[] { hitTrans });

            _needDraw = true;
            _startPos = Input.mousePosition;
        }
        else if (_needDraw && Input.GetMouseButtonUp(0))
        {
            if (onGetRootObjs != null && Vector3.Distance(_startPos, Input.mousePosition) > 1)
            {
                var selectd = SelectObjectRect(_camera, _startPos, Input.mousePosition);
                onGetRootObjs(selectd == null ? null : selectd);
            }
            else if (hitTrans == null && onGetRootObjs != null)
            {
                onGetRootObjs(null);
            }
            _needDraw = false;
        }
    }

    private ISelectable[] SelectObjectRect(Camera camera, Vector3 startPos, Vector3 endPos)
    {
        var dis = camera.transform.position.y - Mathf.Cos(Vector3.Angle(camera.transform.forward, Vector3.down));
        Debug.Log(dis);
        startPos.z = endPos.z = dis;// camera.transform.position.y;//这个值不
        var startPos1 = new Vector3(startPos.x, endPos.y, startPos.z);//与startPos 沿y方向一条线

        var worldStart = Camera.main.ScreenToWorldPoint(startPos);
        var worldEnd = Camera.main.ScreenToWorldPoint(endPos);
        var worldStart1 = Camera.main.ScreenToWorldPoint(startPos1);

        var centerPos = (worldStart + worldEnd) * 0.5f;
        var dir = centerPos - camera.transform.position;
        var quaternion = Quaternion.FromToRotation( Vector3.forward, dir.normalized);
        //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = centerPos;
        //cube.transform.rotation = quaternion;
        var boxSize = new Vector3(Vector3.Distance(worldStart1, worldEnd), Vector3.Distance(worldStart, worldStart1), 100);
        //cube.transform.localScale = boxSize;

        var hits = Physics.BoxCastAll(centerPos, boxSize * 0.5f, dir, quaternion, 0.01f, LayerMask.GetMask(BuildingUtility.MoveItemLayerName));
        List<ISelectable> items = new List<ISelectable>();

        foreach (var item in hits)
        {
            var iItem = item.collider.GetComponent<ISelectable>();
            if (iItem != null)
            {
                items.Add(iItem);
            }
        }
        return items.ToArray();
    }
}

