using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using ListView;

public class BuildTest : MonoBehaviour {
    public BuildingCtrl buildCtrl;
    public ScrollRect scrollRect;
    public BuildItemSelectDrawer quadDrawer;
    void Start () {
        //buildCtrl.Init();
        quadDrawer.onGetRootObjs += buildCtrl.SelectItem;// OnSelectedItems;
    }
    //private void OnSelectedItems(Transform[] trans)
    //{
    //    if (trans != null && trans.Length > 0)
    //    {
    //        var activeItem = trans[0].GetComponent<BuildingItem>();
    //        buildCtrl.ActiveTargetItem(activeItem);
    //        quadDrawer.enabled = false;
    //    }
    //    else
    //    {
    //        buildCtrl.ActiveTargetItem(null);
    //    }
    //}
    private void Update()
    {
        CameraHitUtility.Update(100);
        buildCtrl.Update();
    }
	
}
