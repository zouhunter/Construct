using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public class QuadDrawer : MonoBehaviour {
    private QuadInfo quadInfo;
    public Material green;
    public Material red;

    private void OnPostRender()
    {
        if (quadInfo != null&& quadInfo.quad!=null && quadInfo.quad.Length == 4)
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
    public void DrawQuad(QuadInfo quadInfo)
    {
        this.quadInfo = quadInfo;
    }
    public void ClearQuad()
    {
        quadInfo = null;
    }
}
