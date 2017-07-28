using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public class QuadDrawer : MonoBehaviour {
    private Vector3[] quad { get; set; }
    private bool colorred;
    public Material green;
    public Material red;

    private void OnPostRender()
    {
        if (quad != null && quad.Length == 4)
        {
            GL.PushMatrix();
            if (colorred) red.SetPass(0);
            else green.SetPass(0);

            GL.Begin(GL.LINES);

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
    public void DrawQuad(bool isGreen,Vector3[] quad)
    {
        colorred = !isGreen;
        this.quad = quad;
    }
    public void ClearQuad()
    {
        quad = null;
    }
}
