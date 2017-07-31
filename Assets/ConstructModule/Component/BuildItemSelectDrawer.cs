using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public class BuildItemSelectDrawer : SelectDrawer
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
    protected override void Awake()
    {
        base.Awake();
        InitSelectDrawer<BuildingItem>();
        onGetRootObjs += DrawQuad;
    }
    protected override void OnPostRender()
    {
        base.OnPostRender();
        DrawQuaderInfos();
    }
    private void DrawQuaderInfos()
    {
        if (quadInfos == null) return;
        foreach (var quadInfo in quadInfos)
        {
            if (quadInfo != null && quadInfo.quad != null && quadInfo.quad.Length == 4)
            {
                GL.PushMatrix();
                if (!quadInfo.installAble) red.SetPass(0);
                else green.SetPass(0);

                GL.Begin(GL.LINES);

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
