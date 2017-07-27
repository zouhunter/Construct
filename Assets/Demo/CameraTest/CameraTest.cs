using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using RuntimeGizmos;

    public class CameraTest : MonoBehaviour {
    public DragCamera dragCamera;
    public SelectDrawer drawer;
    public TransormAbleGizmo transGizmo;
    private Vector3 lastTargetPos;
    private Transform[] lastTrans;
    private void OnEnable()
    {
        drawer.onGetRootObjs = (x) =>
        {
            lastTrans = x;
            var root = transGizmo.SetTargets(x);
            if (root != null)
            {
                lastTargetPos = root.position;
                dragCamera.SetTarget(lastTargetPos);
            }
        };
        transGizmo.onTransormingStateChanged = (x) => {
            drawer.enabled = !x;
        };
        drawer.InitSelectDrawer<MeshRenderer>();
    }
   
    private void OnGUI()
    {
        if (GUILayout.Button("2d-3d"))
        {
            //选择打开dragCamera
            dragCamera.SwitchisTopViewOr3D(lastTargetPos);
        }

        if (GUILayout.Button("移动"))
        {
            transGizmo.SetTransType(TransformType.Move);
        }
        if (GUILayout.Button("旋转"))
        {
            transGizmo.SetTransType(TransformType.Rotate);
        }
        if (GUILayout.Button("缩放"))
        {
            transGizmo.SetTransType(TransformType.Scale);
        }
        if (GUILayout.Button("本地-世界"))
        {
            transGizmo.SwitchSpace();

        }
    }
}
