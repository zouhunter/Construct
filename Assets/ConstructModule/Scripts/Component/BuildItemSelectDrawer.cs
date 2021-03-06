﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System;

[RequireComponent(typeof(SelectDrawer))]
public class BuildItemSelectDrawer : MonoBehaviour
{
    private List<QuadInfo> infos = new List<QuadInfo>();
    private QuadInfo[] quadInfos
    {
        get
        {
            if(selected == null)
            {
                return null;
            }
            else
            {
                infos.Clear();
                for (int i = 0; i < selected.Count; i++)
                {
                    if (selected[i]!= null && selected[i].isActiveAndEnabled)
                    {
                        infos.Add(selected[i].quadInfo);
                    }
                }
                return infos.ToArray();
            }
        }
    }
    private List<BuildingItem> selected;
    public event UnityAction<Transform[]> onGetRootObjs;
    private SelectDrawer selectDrawer;
    protected void Awake()
    {
        selectDrawer = GetComponent<SelectDrawer>();
        selectDrawer.onGetRootObjs += OnGetISelectables;
        onGetRootObjs += DrawQuad;
    }
    private void OnGetISelectables(ISelectable[] items)
    {
        if (onGetRootObjs != null)
        {
            if (items == null)
            {
                if(selected != null && selected.Count > 0)
                {
                    onGetRootObjs.Invoke(null);
                }
                else
                {
                    //DoNothing
                }
            }
            else
            {
                var bitems = Array.FindAll<ISelectable>(items, x => x is BuildingItem);
                if (bitems == null||bitems.Length == 0)
                {
                    onGetRootObjs.Invoke(null);
                }
                else
                {
                    onGetRootObjs.Invoke(Array.ConvertAll<ISelectable, Transform>(bitems, x => x.TransformComponent));
                }
            }
        }
    }
    public void SetEnable(bool enable)
    {
        this.enabled = enable;
        selectDrawer.enabled = enabled;
    }
    protected void OnPostRender()
    {
        DrawQuaderInfos();
        DrawQuadTriangles();
    }
    private void DrawQuaderInfos()
    {
        if (quadInfos == null) return;
        foreach (var quadInfo in quadInfos)
        {
            if (quadInfo != null && quadInfo.quad != null && quadInfo.quad.Length == 4)
            {
                GL.PushMatrix();
                selectDrawer.lineMaterial.SetPass(0);// lineMaterial.SetPass(0);
                GL.Begin(GL.LINES);
                if (!quadInfo.installAble) GL.Color(Color.red);
                else GL.Color(Color.green);

                for (int i = 0; i < quadInfo.quad.Length; ++i)
                {
                    if (i + 1 == quadInfo.quad.Length)
                    {
                        GL.Vertex(quadInfo.quad[i]);
                        GL.Vertex(quadInfo.quad[0]);
                    }
                    else
                    {
                        GL.Vertex(quadInfo.quad[i]);
                        GL.Vertex(quadInfo.quad[i + 1]);
                    }
                }
                GL.Vertex(quadInfo.quad[3]);
                GL.Vertex(quadInfo.quad[0]);

                GL.End();
                GL.PopMatrix();
            }
        }
       
    }
    private void DrawQuadTriangles()
    {
        if (quadInfos == null) return;
        foreach (var quadInfo in quadInfos)
        {
            if (quadInfo != null && quadInfo.viewPosTrangles != null && quadInfo.viewPosTrangles.Length % 3 == 0)
            {
                DrawTriangles(quadInfo.viewPosTrangles, Color.blue);
            }
        }
    }
    private void DrawTriangles(Vector3[] lines, Color color)
    {
        selectDrawer.lineMaterial.SetPass(0);
        GL.Begin(GL.TRIANGLES);
        GL.Color(color);

        for (int i = 0; i < lines.Length; i += 3)
        {
            GL.Vertex(lines[i]);
            GL.Vertex(lines[i + 1]);
            GL.Vertex(lines[i + 2]);
        }

        GL.End();
    }
    private void DrawQuad(Transform[] selectItems)
    {
        if (selectItems != null && selectItems.Length > 0)
        {
            selected = new List<global::BuildingItem>();
            //var infos = new List<QuadInfo>();
            for (int i = 0; i < selectItems.Length; i++)
            {
               var item = selectItems[i].GetComponent<BuildingItem>();
                //infos.Add(item.quadInfo);
                selected.Add(item);
            }
            //this.quadInfos = infos.ToArray();
        }
        else
        {
            //quadInfos = null;
            selected = null;
        }
    }
}
