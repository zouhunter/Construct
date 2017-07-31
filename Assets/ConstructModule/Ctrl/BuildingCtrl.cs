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
    public ItemsHolderObj holderObj;
    private List<BuildingItem> allBuildings = new List<BuildingItem>();
    public BuildingItem ActiveItem { get; private set; }
    public const string movePosTag = "MovePos";
    private RaycastHit hit;
    private SyncListItemCreater<BuildItemUI> creater;
    public UnityAction<BuildingItem> onBuildOK;
    private UnityEngine.AI.NavMeshObstacle[] defultobstacles;
    private float timer;
    private float lastDistence = 10;
    public void Update()
    {
        if (ActiveItem != null)
        {
            UpdateNewItemTargetPos();
        }
    }
    public void RemoveBuilding(BuildingItem item)
    {
        if (allBuildings.Contains(item))
        {
            allBuildings.Remove(item);
        }
    }
    public void AddNewBuiliding(BuildingItem item)
    {
        if (!allBuildings.Contains(item))
        {
            allBuildings.Add(item);
        }
    }
    private void UpdateNewItemTargetPos()
    {
        if (CameraHitUtility.GetOneHit(movePosTag, ref hit))
        {
            ActiveItem.UpdateBuilding(hit.point);
            lastDistence = Vector3.Distance(Camera.main.transform.position, hit.point);
            if (ActiveItem.quadInfo.installAble && InputUtility.HaveClickMouseTwice(ref timer, 0, 0.5f))
            {
                BuildingItem item = ActiveItem.GetComponent<BuildingItem>();
                item.buildState = BuildState.normal;
                AddNewBuiliding(item);
                ActiveItem = null;
                if (onBuildOK != null)
                {
                    onBuildOK(item);
                }
            }
        }
        else
        {
            ActiveItem.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, lastDistence));
        }

        if (InputUtility.HaveClickMouseTwice(ref timer, 1, 0.5f))
        {
            BuildingItem item = ActiveItem.GetComponent<BuildingItem>();
            RemoveBuilding(item);
            GameObject.Destroy(ActiveItem.gameObject);
            ActiveItem = null;
            if (onBuildOK != null)
            {
                onBuildOK(null);
            }
        }
    }
    internal void Init()
    {
        creater = new SyncListItemCreater<BuildItemUI>(parent, prefab);
        CreateDefult();
        defultobstacles = GameObject.FindObjectsOfType<UnityEngine.AI.NavMeshObstacle>();
    }
    public void ActiveTargetItem(BuildingItem item)
    {
        //正在建造中无法选择其他对象
        if(ActiveItem!= null && ActiveItem.buildState == BuildState.inbuild)
        {
            return;
        }
        else
        {
            ActiveItem = item;
            if (ActiveItem != null)
               ActiveItem.buildState = BuildState.inbuild;
        }
       
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
        var item = GameObject.Instantiate(hold).GetComponent<BuildingItem>();
        ActiveTargetItem(item);
    }
    private bool IgnoreObstacle(Vector3 targetPath)
    {
        foreach (var item in defultobstacles)
        {

        }
        foreach (var item in allBuildings)
        {
            var ob = item.GetComponent<UnityEngine.AI.NavMeshObstacle>();
            if (IsPointInBox(targetPath, ob.center, ob.size))
            {
                return false;
            }
        }
        return true;
    }
    private static bool IsPointInBox(Vector3 point, Vector3 centerPos, Vector3 size)
    {
        var halfBoxWeight = size.x * 0.5f;
        var halfBoxLength = size.z * 0.5f;
        var halfBoxHeight = size.y * 0.5f;
        var dir = point - centerPos;
        if (Mathf.Abs(dir.x) - halfBoxWeight < 0 && Mathf.Abs(dir.y) - halfBoxHeight < 0 && Mathf.Abs(dir.z) - halfBoxLength < 0)
        {
            return true;
        }
        return false;
    }
}

