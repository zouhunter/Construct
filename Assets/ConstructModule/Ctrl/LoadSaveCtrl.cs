using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System;
/// <summary>
/// 暂时从本地加载预制体
/// </summary>
public class LoadSaveCtrl  {
    private ItemsHolderObj holderObj;

    public LoadSaveCtrl(ItemsHolderObj holderObj)
    {
        this.holderObj = holderObj;
    }
    internal string RecordToJson(NaviPoint[] items)
    {
        var doc = new DBNaviPointDocument();
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            doc.records.Add(item.transform.position);
        }
        return JsonUtility.ToJson(doc);
    }
    internal string RecordToJson(BuildingItem[] items)
    {
        var doc = new DBDeviceRecordDocument();
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var iteminfo = holderObj.ItemHoldList.Find(x =>x.itemName == item.deviceName);
            if (iteminfo != null)
            {
                var record = new DBDeviceRecord();
                record.deviceName = iteminfo.itemName;
                record.position = item.transform.position;
                record.rotation = item.transform.eulerAngles;
                record.localScale = item.transform.localScale;
                doc.records.Add(record);
            }
        }
        return JsonUtility.ToJson(doc);
    }

    internal BuildingItem[] LoadBuildItemsFromJson(string json)
    {
        var itemList = new List<BuildingItem>();
        var doc = JsonUtility.FromJson<DBDeviceRecordDocument>(json);
        for (int i = 0; i < doc.records.Count; i++)
        {
            var iteminfo = holderObj.ItemHoldList.Find(x => x.itemName == doc.records[i].deviceName);
            var obj = GameObject.Instantiate(iteminfo.prefab);
            UnDoUtil.RecordStep(new CreateStepRecord(obj.GetComponent<BuildingItem>()));
            itemList.Add(obj.GetComponent<BuildingItem>());
            obj.transform.position = doc.records[i].position;
            obj.transform.eulerAngles = doc.records[i].rotation;
            obj.transform.localScale = doc.records[i].localScale;
            UnDoUtil.RecordStep(new TransformStepRecord(obj.GetComponent<BuildingItem>()));
        }
        return itemList.ToArray();
    }

    internal NaviPoint[] LoadNaviPointsFromJson(string json)
    {
        var itemList = new List<NaviPoint>();
        var doc = JsonUtility.FromJson<DBNaviPointDocument>(json);
        for (int i = 0; i < doc.records.Count; i++)
        {
            var obj = GameObject.Instantiate(holderObj.naviPoint);
            obj.Id = i + 1;
            obj.name = obj.Id.ToString();
            itemList.Add(obj);
            obj.transform.position = doc.records[i];
        }
        return itemList.ToArray();
    }
}
