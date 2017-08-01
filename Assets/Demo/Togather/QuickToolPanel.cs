using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System;

public class QuickToolPanel : MonoBehaviour
{
    public ItemsHolderObj holderObj;
    private LoadSaveCtrl loadSave;

    string filePath = "";
    [SerializeField]
    private Button m_back;
    [SerializeField]
    private Button m_forward;
    [SerializeField]
    private Button m_load;
    [SerializeField]
    private Button m_save;
    [SerializeField]
    private Toggle m_top;

    [SerializeField]
    private Button m_naviPoint;
    private DragCamera m_DgCamera;
    private Vector3 centerView;
    private void Awake()
    {
        m_back.onClick.AddListener(OnMyBackClicked);
        m_forward.onClick.AddListener(OnMyForwardClicked);
        loadSave = new global::LoadSaveCtrl(holderObj);
        filePath = Application.dataPath + "/Demo/LoadSaveTest/record.json";
        m_save.onClick.AddListener(RecordToJson);
        m_load.onClick.AddListener(LoadFromJson);
        m_top.onValueChanged.AddListener(OnChangeView);
        m_naviPoint.onClick.AddListener(OnCreateNaviPoint);
    }

    private void OnCreateNaviPoint()
    {
        SceneMain.Current.InvokeEvents(TogatherEvents.onCreatePoint);
    }

    private void Start()
    {
        m_DgCamera = Camera.main.GetComponent<DragCamera>();
        SceneMain.Current.RegisterEvent<Vector3>(TogatherEvents.onCenterViewChanged, (x) => centerView = (Vector3)x);
    }

    private void OnChangeView(bool arg0)
    {
        m_DgCamera.SwitchisTopViewOr3D(centerView);
    }

    private void OnMyBackClicked()
    {
        UnDoUtility.UnDoOneStep();
    }
    private void OnMyForwardClicked()
    {
        UnDoUtility.ReDoOneStep();
    }

    private void RecordToJson()
    {
        var items = FindObjectsOfType<BuildingItem>();
        var json = loadSave.RecordToJson(items);
        Debug.Log(json);
        System.IO.File.WriteAllText(filePath, json);
    }
    private void LoadFromJson()
    {
        var json = System.IO.File.ReadAllText(filePath);
        BuildingItem[] items = loadSave.ReadFromJson(json);
        if (items != null)
            foreach (var item in items)
            {
                Debug.Log(item.name);
            }
    }
}
