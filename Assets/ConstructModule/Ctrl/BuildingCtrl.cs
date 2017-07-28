using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using ListView;
using System;

[System.Serializable]
public class BuildingCtrl
{
    public Transform parent;
    public BuildItemUI prefab;

    private ItemsHolderObj holderObj;
    private List<BuildingItem> allBuildings = new List<BuildingItem>();
    private GameObject activeItem;
    public const string movePosTag = "MovePos";
    private RaycastHit hit;
    private SyncListItemCreater<BuildItemUI> creater;
    public UnityAction<BuildingItem> onBuildOK;
    public UnityAction<bool> onMoveStateChanged;
    private float timer;
    private float lastDistence = 10;
    public bool Update()
    {
        if (activeItem != null)
        {
            UpdateNewItemTargetPos();
            return true;
        }
        return false;
    }
    private void UpdateNewItemTargetPos()
    {
        if (HitUtility.GetOneHit(movePosTag, ref hit))
        {
            activeItem.transform.position = hit.point;
            lastDistence = Vector3.Distance(Camera.main.transform.position, hit.point);
        }
        else
        {
            activeItem.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, lastDistence));
        }
        if (InputUtility.HaveClickMouseTwice(ref timer, 0))
        {
            BuildingItem item = activeItem.GetComponent<BuildingItem>();
            AddNewBuiliding(item);
            activeItem = null;
            if (onBuildOK != null)
            {
                onBuildOK(item);
            }
        }
        else if (InputUtility.HaveClickMouseTwice(ref timer, 1))
        {
            BuildingItem item = activeItem.GetComponent<BuildingItem>();
            RemoveBuilding(item);
            GameObject.Destroy(activeItem);
            activeItem = null;
            if (onBuildOK != null){
                onBuildOK(null);
            }
        }
    }
    internal void Init(ItemsHolderObj holderObj)
    {
        this.holderObj = holderObj;
        creater = new SyncListItemCreater<BuildItemUI>(parent, prefab);
        CreateDefult();
    }
    public void ActiveItem(BuildingItem item)
    {
        activeItem = item == null ? null : item.gameObject;
    }
    private void CreateDefult()
    {
        creater.CreateItems(holderObj.ItemHoldList.Count);
        for (int i = 0; i < creater.CreatedItems.Count; i++)
        {
            var item = creater.CreatedItems[i];
            item.InitData(holderObj.ItemHoldList[i]);
            item.onButtonClicked = OnClickItem;
        }
    }
    private void OnClickItem(GameObject hold)
    {
        activeItem = GameObject.Instantiate(hold);
    }
    private void RemoveBuilding(BuildingItem item)
    {
        if (allBuildings.Contains(item))
        {
            allBuildings.Remove(item);
        }
    }
    private void AddNewBuiliding(BuildingItem item)
    {
        if (!allBuildings.Contains(item))
        {
            allBuildings.Add(item);
        }
    }

}

