using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using RuntimeGizmos;

    public class CameraTest : MonoBehaviour {
    public DragCamera dragCamera;
    public BuildingViewCamera buildCamera;
    public SelectDrawer drawer;
    public TransormAbleGizmo transGizmo;
    private Vector3 lastTargetPos;
    private void OnEnable()
    {
        drawer.onGetRootObjs = (x) =>
        {
            buildCamera.target = transGizmo.SetTargets(x.ToArray());
            lastTargetPos = buildCamera.target.position;
        };
        drawer.onLostSelect = transGizmo.DeleteTarget;
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
            dragCamera.SwitchisTopViewOr3D(lastTargetPos);
        }
        if (GUILayout.Button("相机切换"))
        {
            //先切换为正常视角
            if (dragCamera.enabled && dragCamera.IsTopView){
                dragCamera.SwitchisTopViewOr3D(lastTargetPos);
            }
            dragCamera.enabled = !dragCamera.enabled;
            buildCamera.enabled = !dragCamera.enabled;
        }
    }
}
