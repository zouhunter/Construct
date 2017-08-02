using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System;

[RequireComponent(typeof(SelectDrawer))]
public class NaviPointSelectDrawer : MonoBehaviour
{
    private List<Vector3[]> infos = new List<Vector3[]>();
    private List<Vector3[]> quadInfos
    {
        get
        {
            if (selected == null)
            {
                return null;
            }
            else
            {
                infos.Clear();
                for (int i = 0; i < selected.Count; i++)
                {
                    if (selected[i] != null && selected[i].isActiveAndEnabled)
                    {
                        infos.Add(selected[i].quad);
                    }
                }
                return infos;
            }
        }
    }
    private List<NaviPoint> selected;
    public event UnityAction<Transform[]> onGetRootObjs;
    private SelectDrawer selectDrawer;
    protected void Awake()
    {
        selectDrawer = GetComponent<SelectDrawer>();
        selectDrawer.onGetRootObjs += OnGetISelectables;
        onGetRootObjs += DrawQuad;
    }
    public void SetEnableState(bool enable)
    {
        this.enabled = enabled;
        selectDrawer.enabled = enabled;
    }
    private void OnGetISelectables(ISelectable[] items)
    {
        if (onGetRootObjs != null)
        {
            if (items == null)
            {
                if (selected != null && selected.Count > 0)
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
                var bitems = Array.FindAll<ISelectable>(items, x => x is NaviPoint);
                if (bitems == null || bitems.Length == 0)
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
    protected void OnPostRender()
    {
        DrawQuaderInfos();
    }
    private void DrawQuaderInfos()
    {
        if (quadInfos == null) return;
        foreach (var quad in quadInfos)
        {
            if (quad != null && quad.Length == 4)
            {
                GL.PushMatrix();
                selectDrawer.lineMaterial.SetPass(0);// lineMaterial.SetPass(0);
                GL.Begin(GL.LINES);
                GL.Color(Color.green);

                for (int i = 0; i < quad.Length; ++i)
                {
                    if (i + 1 == quad.Length)
                    {
                        GL.Vertex(quad[i]);
                        GL.Vertex(quad[0]);
                    }
                    else
                    {
                        GL.Vertex(quad[i]);
                        GL.Vertex(quad[i + 1]);
                    }
                }
                GL.Vertex(quad[3]);
                GL.Vertex(quad[0]);

                GL.End();
                GL.PopMatrix();
            }
        }

    }

    private void DrawQuad(Transform[] selectItems)
    {
        if (selectItems != null && selectItems.Length > 0)
        {
            selected = new List<global::NaviPoint>();
            //var infos = new List<QuadInfo>();
            for (int i = 0; i < selectItems.Length; i++)
            {
                var item = selectItems[i].GetComponent<NaviPoint>();
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
