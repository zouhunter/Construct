using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using ListView;

public class BuildTest : MonoBehaviour {
    public SelectDrawer drawer;
    public BuildingCtrl buildCtrl;
    public ScrollRect scrollRect;
    public BuildItemSelectDrawer quadDrawer;
    void Start () {
        buildCtrl.Init();
        drawer.InitSelectDrawer<BuildingItem>();
        drawer.onGetRootObjs += OnSelectedItems;
    }
    private void OnSelectedItems(Transform[] trans)
    {
        if (trans != null && trans.Length > 0)
        {
            var activeItem = trans[0].GetComponent<BuildingItem>();
            buildCtrl.ActiveTargetItem(activeItem);
            drawer.enabled = false;
        }
        else
        {
            buildCtrl.ActiveTargetItem(null);
        }
    }
    private void Update()
    {
        CameraHitUtility.Update(100);
        buildCtrl.Update();
    }
	
}
