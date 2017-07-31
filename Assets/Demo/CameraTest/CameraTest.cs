using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using RuntimeGizmos;

    public class CameraTest : MonoBehaviour {
    public DragCamera dragCamera;
    public SelectDrawer drawer;
    public GizmoBehaviour transGizmo;
    private Vector3 lastTargetPos;
    //private Transform[] lastTrans;
    private void OnEnable()
    {
        drawer.onGetRootObjs += OnSelectSelected;
        transGizmo.targetCtrl.onTransormingStateChanged = (x) => {
            drawer.enabled = !x;
        };
        drawer.InitSelectDrawer<MeshRenderer>();
    }
    private void OnSelectSelected(Transform[] x)
    {
        if(x== null)
        {
            transGizmo.targetCtrl.SetTargets(null);
        }
        else
        {
            //lastTrans = x;
            var root = transGizmo.targetCtrl.SetTargets(System.Array.ConvertAll<Transform,BuildingItem>(x,y=>y.GetComponent<BuildingItem>()));
            if (root != null)
            {
                lastTargetPos = root.position;
                dragCamera.SetTarget(lastTargetPos);
            }
        }
       
    }
    private void OnDisable()
    {
        drawer.onGetRootObjs += OnSelectSelected;
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
            transGizmo.targetCtrl.SetTransType(TransformType.Move);
        }
        if (GUILayout.Button("旋转"))
        {
            transGizmo.targetCtrl.SetTransType(TransformType.Rotate);
        }
        if (GUILayout.Button("缩放"))
        {
            transGizmo.targetCtrl.SetTransType(TransformType.Scale);
        }
        if (GUILayout.Button("本地-世界"))
        {
            transGizmo.targetCtrl.SwitchSpace();

        }
    }
}
