using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class DBDeviceRecordDocument
{
    public string name;
    public string userid;
    public string username;
    public string time;
    public List<DBDeviceRecord> records = new List<DBDeviceRecord>();
}
