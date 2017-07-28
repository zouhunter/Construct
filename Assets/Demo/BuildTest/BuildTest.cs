using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using ListView;

public class BuildTest : MonoBehaviour {
    public SelectDrawer drawer;
    private BuildingItem activeItem;
    public BuildingCtrl buildCtrl;
    public ItemsHolderObj holderObj;
    public ScrollRect scrollRect;

    void Start () {
        buildCtrl.Init(holderObj);
        drawer.InitSelectDrawer<BuildingItem>();
        buildCtrl.onBuildOK = (x) => {
            drawer.enabled = true;
        };
            
        buildCtrl.onMoveStateChanged = (x) => {
            drawer.enabled = !x;
            if(!x) activeItem.SetBuildState(BuildState.Normal);
        };
        drawer.onGetRootObjs = (x) =>
        {
            if(x != null && x.Length > 0)
            {
                activeItem = x[0].GetComponent<BuildingItem>();
                activeItem.SetBuildState(BuildState.Inbuild);
                buildCtrl.ActiveItem(activeItem);
                drawer.enabled = false;
            }
            else
            {
                buildCtrl.ActiveItem(null);
            }
        };
    }
    private void Update()
    {
        HitUtility.Update(100);

        if (buildCtrl.Update())
        {
            scrollRect.enabled = false;
        }
        else
        {
            scrollRect.enabled = true;
        }
    }
   
	// Update is called once per frame
	
}
