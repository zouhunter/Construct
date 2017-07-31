using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using ListView;
using System;
using RuntimeGizmos;

public class LocalListPanel : MonoBehaviour
{
    public BuildingCtrl buildCtrl;
    public BuildItemSelectDrawer quadDrawer;
    public GizmoBehaviour transGizmo;
    private BuildingItem[] activeItems;
    public DragCamera dragCamera;
    private Vector3 lastTargetPos;
    private float timer = 0;
    void Start()
    {
        buildCtrl.Init();
        quadDrawer.onGetRootObjs += SelectItem;
        transGizmo.targetCtrl.onTransormingStateChanged = (x) =>
        {
            quadDrawer.enabled = !x;
        };
    }

    private void SelectItem(Transform[] arg0)
    {
        if (arg0 != null && arg0.Length > 0)
        {
            //防止放下的瞬间又拿起
            if(Array.Find(arg0,x=>x.GetComponent<BuildingItem>() == buildCtrl.ActiveItem) != null){
                return;
            }
            if (InputUtility.HaveExecuteTwicePerSecond(ref timer, 0.5f))
            {
                OnSelectedItems(null);
                foreach (var bitem in arg0)
                {
                    var item = bitem.GetComponent<BuildingItem>();
                    if (item.buildState == BuildState.normal) buildCtrl.ActiveTargetItem(item);
                }
            }
            else
            {
                OnSelectedItems(arg0);
            }
        }
        else
        {
            buildCtrl.ActiveTargetItem(null);
            OnSelectedItems(null);
        }
    }
    private void OnSelectedItems(Transform[] trans)
    {
        if (trans == null || trans.Length == 0) {
            transGizmo.targetCtrl.SetTargets(null);
        }
        else
        {
            //lastTrans = x;
            var root = transGizmo.targetCtrl.SetTargets(System.Array.ConvertAll<Transform, BuildingItem>(trans, x => x.GetComponent<BuildingItem>()));
            if (root != null)
            {
                lastTargetPos = root.position;
                dragCamera.SetTarget(lastTargetPos);
            }
            activeItems = System.Array.ConvertAll<Transform, BuildingItem>(trans, x => x.GetComponent<BuildingItem>());
        }
        
    }

    private void Update()
    {
        CameraHitUtility.Update(100);
        buildCtrl.Update();
        if (activeItems != null)
        {
            foreach (var item in activeItems)
            {
                if (item != null && item.isActiveAndEnabled)
                {
                    item.UpdateQuad();
                }
            }
        }
    }

}
