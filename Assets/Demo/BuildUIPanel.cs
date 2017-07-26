using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using RuntimeGizmos;

    public class BuildUIPanel : MonoBehaviour {
    public DragCamera dragCamera;
    public BuildingViewCamera buildCamera;
    public SelectDrawer drawer;
    public TransformGizmo transGizmo;
    private void OnEnable()
    {
        drawer.onGetRootObjs = (x) =>
        {
            transGizmo.SetTargets(x.ToArray());
        };
    }
    private void OnGUI()
    {
        if (GUILayout.Button("Switch"))
        {
            dragCamera.SwitchisTopViewOr3D();
        }
        if (GUILayout.Button("相机切换"))
        {
            dragCamera.enabled = !dragCamera.enabled;
            buildCamera.enabled = !dragCamera.enabled;
        }
        if (GUILayout.Button("Select"))
        {
            drawer.InitSelectDrawer<MeshRenderer>();
        }
    }
}
