using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using ListView;

public class BuildTest : MonoBehaviour {
    public SelectablePlane selectPanel;
    public SelectDrawer drawer;
    private BuildingItem activeItem;
    private SyncListItemCreater<BuildItemUI> creater;
    public Transform parent;
    public BuildItemUI prefab;
    public ItemsHolderObj holderObj;
    public ScrollRect scrollRect;
    private GameObject instence;

   void Start () {
        creater = new SyncListItemCreater<BuildItemUI>(parent, prefab);
        drawer.InitSelectDrawer<BuildingItem>();
        selectPanel.onMoveStateChanged = (x) => {
            drawer.enabled = !x;
            if(!x) activeItem.SetBuildState(BuildState.Normal);
        };
        drawer.onGetRootObjs = (x) =>
        {
            if(x != null && x.Length > 0)
            {
                activeItem = x[0].GetComponent<BuildingItem>();
                activeItem.SetBuildState(BuildState.Inbuild);
                selectPanel.SetTarget(activeItem);
                drawer.enabled = false;
            }
            else
            {
                selectPanel.SetTarget(null);
            }
        };
        CreateUIWithObjItem();
    }
    private void Update()
    {
        if (instence != null)
        {
            scrollRect.enabled = false;
        }
        else
        {
            scrollRect.enabled = true;
        }
    }
    void CreateUIWithObjItem()
    {
        creater.CreateItems(holderObj.ItemHoldList.Count);
        for (int i = 0; i < creater.CreatedItems.Count; i++)
        {
            var item = creater.CreatedItems[i];
            item.InitData(holderObj.ItemHoldList[i]);
            item.onButtonClicked = OnClickItem;
        }
    }
	// Update is called once per frame
	void OnClickItem (GameObject hold) {
        //if (instence == null)
        //{
        instence = Instantiate(hold);
            drawer.onGetRootObjs.Invoke(new Transform[] { instence.transform });
        //}
    }
}
