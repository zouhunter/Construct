using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using ListView;

public class BuildTest : MonoBehaviour {
    public SelectDrawer drawer;
    public BuildingCtrl buildCtrl;
    public ItemsHolderObj holderObj;
    public ScrollRect scrollRect;
    public QuadDrawer quadDrawer;
    void Start () {
        buildCtrl.Init(holderObj);
        drawer.InitSelectDrawer<BuildingItem>();
        drawer.onGetRootObjs = OnSelectedItems;
        buildCtrl.onBuildOK = (x) =>
        {
            quadDrawer.ClearQuad();
        };
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

        if (buildCtrl.Update())
        {
            quadDrawer.DrawQuad(buildCtrl.ActiveItem.quadInfo);
            drawer.enabled = false;
            scrollRect.enabled = false;
        }
        else
        {
            drawer.enabled = true;
            scrollRect.enabled = true;
        }
    }
	
}
