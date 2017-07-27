using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;
public class SelectDrawer : MonoBehaviour
{
    public Material green;
    public Material red;
    private Camera _camera;
    private Vector3 _startPos;
    private bool _needDraw;

    public Type _type;
    public UnityAction<Transform[]> onGetRootObjs;
    private Transform hitTrans = null;
    public void InitSelectDrawer<T>() where T : Component
    {
        this._type = typeof(T);
    }

    private void Awake()
    {
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
        green.SetPass(0);

        //坐标转化
        GL.LoadPixelMatrix();
        GL.Begin(GL.LINES);

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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            _needDraw = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_type == null)
            {
                _needDraw = true;
            }
            else
            {
                RaycastHit hitInfo;
                var hit = Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hitInfo);
                
                if (hit && hitInfo.collider != null && hitInfo.collider.GetComponent(_type) != null)
                {
                    _needDraw = true;
                    hitTrans = hitInfo.collider.transform;
                }
                else
                {
                    hitTrans = null;
                    _needDraw = true;
                }

                if (onGetRootObjs != null)
                    onGetRootObjs(hitTrans == null ? null : new Transform[] { hitTrans });
            }
            if (_needDraw)
            {
                _startPos = Input.mousePosition;
            }
        }
        else if (_needDraw && Input.GetMouseButtonUp(0))
        {
            if (_type != null && onGetRootObjs != null && Vector3.Distance(_startPos, Input.mousePosition) > 1)
            {
                var selectd = SelectObjectRect(_camera, _type, _startPos, Input.mousePosition);
                onGetRootObjs(selectd == null ? null : selectd.ToArray());
            }
            else if(hitTrans == null && onGetRootObjs != null)
            {
                onGetRootObjs(null);
            }
            _needDraw = false;
        }
    }

    private static List<Transform> SelectObjectRect(Camera camera, Type type, Vector3 startPos, Vector3 endPos)
    {
        List<Transform> items = new List<Transform>();
        var objs = UnityEngine.Object.FindObjectsOfType(type) as Component[];
        foreach (var item in objs)
        {
            var trans = item.transform;
            var viewPos = camera.WorldToScreenPoint(trans.position);
            if (IsPointInBox(viewPos, startPos, endPos))
            {
                items.Add(item.transform);
            }
        }
        return items;
    }

    private static bool IsPointInBox(Vector2 point, Vector2 startPoint, Vector2 endPoint)
    {
        var boxCenter = (startPoint + endPoint) * 0.5f;
        var halfBoxWeight = Mathf.Abs(startPoint.x - endPoint.x) * 0.5f;
        var halfBoxHeight = Mathf.Abs(startPoint.y - endPoint.y) * 0.5f;
        var dir = point - boxCenter;
        if (Mathf.Abs(dir.x) - halfBoxWeight < 0 && Mathf.Abs(dir.y) - halfBoxHeight < 0)
        {
            return true;
        }
        return false;
    }
}

