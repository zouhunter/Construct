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
    public Transform parent;
    public BuildItemUI prefab;
    private ItemsHolderObj holderObj;

 
    private SyncListItemCreater<BuildItemUI> creater;

    public event UnityAction<BuildItemHold> onBuildUIItemClicked;
    void OnEnable()
    {
        creater = new SyncListItemCreater<BuildItemUI>(parent, prefab);
        CreateDefult();
    }
    public void InitLocalListPanel(ItemsHolderObj holderObj)
    {
        this.holderObj = holderObj;
    }

    private void CreateDefult()
    {
        creater.CreateItems(holderObj.ItemHoldList.Count);
        for (int i = 0; i < creater.CreatedItems.Count; i++)
        {
            var item = creater.CreatedItems[i];
            item.InitData(holderObj.ItemHoldList[i]);
            item.onButtonClicked = OnBuildUIItemClicked; 
        }
    }
    private void OnBuildUIItemClicked(BuildItemHold hold)
    {
        if (onBuildUIItemClicked != null) onBuildUIItemClicked.Invoke(hold);
    }

   
}
