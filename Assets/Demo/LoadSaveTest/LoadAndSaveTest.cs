using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public class LoadAndSaveTest : MonoBehaviour {
    public ItemsHolderObj holderObj;
    private LoadSaveCtrl loadSave;
    string filePath = "";
    
    private void OnEnable()
    {
        loadSave = new global::LoadSaveCtrl(holderObj);
        filePath = Application.dataPath + "/Demo/LoadSaveTest/record.json";
    }
    private void OnGUI()
    {
        if (GUILayout.Button("Record"))
        {
            RecordToJson();
        }
        if (GUILayout.Button("Load"))
        {
            LoadFromJson();
        }
    }
    void RecordToJson()
    {
        var items = FindObjectsOfType<BuildingItem>();
        var json = loadSave.RecordToJson(items);
        Debug.Log(json);
        System.IO.File.WriteAllText(filePath, json);
    }
    void LoadFromJson()
    {
        var json = System.IO.File.ReadAllText(filePath);
        BuildingItem[] items = loadSave.LoadBuildItemsFromJson(json);
        if(items!= null)
        foreach (var item in items)
        {
            Debug.Log(item.name);
        }
    }
}
