using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System;

public class QuickToolPanel : MonoBehaviour
{
    private LoadSaveCtrl loadSave;

    string itemsfilePath = "";
    string naviPointsfilePath = "";
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
    private Button m_loadRoad;
    [SerializeField]
    private Button m_saveRoad;
    [SerializeField]
    private Button m_naviPoint;
    private DragCamera m_DgCamera;
    private Vector3 centerView;
    public event UnityAction<NaviPoint[]> onLoadNaviPoints;
    public event UnityAction onCreateNaviPoint;
    private void Awake()
    {
        m_back.onClick.AddListener(OnMyBackClicked);
        m_forward.onClick.AddListener(OnMyForwardClicked);
        m_save.onClick.AddListener(RecordToJson);
        m_load.onClick.AddListener(LoadFromJson);
        m_top.onValueChanged.AddListener(OnChangeView);
        m_naviPoint.onClick.AddListener(OnCreateNaviPoint);
        m_loadRoad.onClick.AddListener(OnLoadRoadFromJson);
        m_saveRoad.onClick.AddListener(OnRecordRoadToJson);
        itemsfilePath = Application.dataPath + "/Demo/LoadSaveTest/items.json";
        naviPointsfilePath = itemsfilePath.Replace("items", "points");
    }

    private void OnRecordRoadToJson()
    {
        var items = FindObjectsOfType<NaviPoint>();
        var json = loadSave.RecordToJson(items);
        Debug.Log(json);
        System.IO.File.WriteAllText(naviPointsfilePath, json);
    }

    private void OnLoadRoadFromJson()
    {
        var json = System.IO.File.ReadAllText(naviPointsfilePath);
        NaviPoint[] items = loadSave.LoadNaviPointsFromJson(json);
        if (onLoadNaviPoints != null) onLoadNaviPoints.Invoke(items);
        if (items != null)
            foreach (var item in items)
            {
                Debug.Log(item.name);
            }
    }

    public void InitLoadSaveCtrl(ItemsHolderObj holderObj)
    {
        loadSave = new global::LoadSaveCtrl(holderObj);
    }

    private void OnCreateNaviPoint()
    {
       if(onCreateNaviPoint!= null)  onCreateNaviPoint.Invoke();
        //TogatherEvents.Instence.InvokeEvents(TogatherEvents.onCreatePoint);
    }

    private void Start()
    {
        m_DgCamera = Camera.main.GetComponent<DragCamera>();
        PlacementEvents.Instence.RegisterEvent<Vector3>(PlacementEvents.onCenterViewChanged, (x) => centerView = (Vector3)x);
    }

    private void OnChangeView(bool arg0)
    {
        m_DgCamera.SwitchisTopViewOr3D(centerView);
    }

    private void OnMyBackClicked()
    {
        UnDoUtil.UnDoOneStep();
    }
    private void OnMyForwardClicked()
    {
        UnDoUtil.ReDoOneStep();
    }

    private void RecordToJson()
    {
        var items = FindObjectsOfType<BuildingItem>();
        var json = loadSave.RecordToJson(items);
        Debug.Log(json);
        System.IO.File.WriteAllText(itemsfilePath, json);
    }
    private void LoadFromJson()
    {
        var json = System.IO.File.ReadAllText(itemsfilePath);
        BuildingItem[] items = loadSave.LoadBuildItemsFromJson(json);
        if (items != null)
            foreach (var item in items)
            {
                Debug.Log(item.name);
            }
    }
}
