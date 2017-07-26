﻿using UnityEngine;
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
    public UnityAction<List<Transform>> onGetRootObjs;
    public UnityAction onLostSelect;

    public void InitSelectDrawer<T>() where T : Component
    {
        this._type = typeof(T);
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
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
                if (!hit)
                {
                    _needDraw = true;
                }
                else
                {
                    if (hitInfo.collider != null&& hitInfo.collider.GetComponent(_type) != null)
                    {
                        if (onGetRootObjs != null)
                            onGetRootObjs(new List<Transform>() { hitInfo.transform });
                    }
                    else
                    {
                        _needDraw = true;
                        if (onLostSelect != null)
                            onLostSelect();
                    }
                }
            }
            if (_needDraw)
            {
                _startPos = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _needDraw = false;
            if (_type != null && onGetRootObjs != null)
            {
                var selectd = SelectObjectRect(_camera, _type, _startPos, Input.mousePosition);
                if (selectd.Count > 0)
                {
                    onGetRootObjs(selectd);
                } 
            }
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

    private static bool IsPointInBox(Vector3 point, Vector3 startPoint, Vector3 endPoint)
    {
        var boxCenter = (startPoint + endPoint) * 0.5f;
        var halfBoxWeight = Mathf.Abs(startPoint.x - endPoint.x) * 0.5f;
        var halfBoxLength = Mathf.Abs(startPoint.z - endPoint.z) * 0.5f;
        var halfBoxHeight = Mathf.Abs(startPoint.y - endPoint.y) * 0.5f;
        var dir = point - boxCenter;
        if (Mathf.Abs(dir.x) - halfBoxWeight < 0 &&
            Mathf.Abs(dir.y) - halfBoxHeight < 0 &&
            Mathf.Abs(dir.z) - halfBoxHeight < 0)
        {
            return true;
        }
        return false;
    }
}

