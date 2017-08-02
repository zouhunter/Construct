using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "生成/设备列表")]
public class ItemsHolderObj:ScriptableObject {
    public string key;
    public NaviPoint naviPoint;
    public List<BuildItemHold> ItemHoldList = new List<BuildItemHold>();
}
