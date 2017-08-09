using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
[System.Serializable]
public class DBDeviceRecord {
    public string deviceName;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 localScale;
    public Vector3 lookPoint;
    public void Record(BuildingItem item)
    {
        this.deviceName = item.deviceName;
        this.position = item.transform.position;
        this.rotation = item.transform.eulerAngles;
        this.localScale = item.transform.localScale;
        this.lookPoint = item.GetViewPos();
    }
    public void UnRecord(BuildingItem item)
    {
        item.deviceName = this.deviceName;
        item.transform.position = this.position;
        item.transform.eulerAngles = this.rotation;
        item.transform.localScale = this.localScale;
        item.SetViewPos(this.lookPoint);
    }
}
