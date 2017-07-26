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
            buildCamera.target = transGizmo.SetTargets(x.ToArray());
        };
        drawer.onLostSelect = transGizmo.DeleteTargets;
        transGizmo.onTransormingStateChanged = (x) => {
            drawer.enabled = !x;
        };
        drawer.InitSelectDrawer<MeshRenderer>();
    }
    private void OnGUI()
    {
        if (GUILayout.Button("Switch"))
        {
            //选择打开dragCamera
            if (buildCamera.enabled)
            {
                dragCamera.enabled = true;
                buildCamera.enabled = false;
            }
            dragCamera.SwitchisTopViewOr3D(buildCamera.target);
        }
        if (GUILayout.Button("相机切换"))
        {
            //先切换为正常视角
            if (dragCamera.enabled && dragCamera.IsTopView){
                dragCamera.SwitchisTopViewOr3D(buildCamera.target);
            }
            dragCamera.enabled = !dragCamera.enabled;
            buildCamera.enabled = !dragCamera.enabled;
        }
    }
}
