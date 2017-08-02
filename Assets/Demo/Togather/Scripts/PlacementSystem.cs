using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using RuntimeGizmos;
using System;

public class PlacementSystem : MonoBehaviour
{
    [Header("视角相机及绘制渲染的预制体")]
    public GameObject viewCameraPrefab;
    [Header("保存预制体对象")]
    public ItemsHolderObj itemsHoldObj;

    [Header("受控制面板")]
    public QuickToolPanel toolPanel;
    public LocalListPanel localListPanel;
    public DeviceInfoPanel infoPanel;

    private NaviPointCtrl naviPointCtrl;
    private BuildingCtrl buildCtrl;

    private NaviPointSelectDrawer naviPointSelecter;
    private BuildItemSelectDrawer buildSelecter;
    private GizmoBehaviour transGizmo;
    private DragCamera dragCamera;

    //private Camera viewCamera;

    private void Awake()
    {
        PlacementEvents.Instence.Reset();

        InitEnviroment();
        InitController();
        RegisterEvents();
        InitPanels();
    }

    private void InitEnviroment()
    {
        var cameraObj = Instantiate(viewCameraPrefab) as GameObject;
        naviPointSelecter = cameraObj.GetComponent<NaviPointSelectDrawer>();
        buildSelecter = cameraObj.GetComponent<BuildItemSelectDrawer>();
        transGizmo = cameraObj.GetComponent<GizmoBehaviour>();
        dragCamera = cameraObj.GetComponent<DragCamera>();
    }

    private void InitController()
    {
        naviPointCtrl = new NaviPointCtrl(itemsHoldObj.naviPoint);
        buildCtrl = new BuildingCtrl(transGizmo,dragCamera);
    }

    private void InitPanels()
    {
        localListPanel.InitLocalListPanel(itemsHoldObj);
        localListPanel.onBuildUIItemClicked += buildCtrl.CreateBuildItem; 
        toolPanel.InitLoadSaveCtrl(itemsHoldObj);
        toolPanel.onCreateNaviPoint += naviPointCtrl.CreateItem;
        toolPanel.onLoadNaviPoints += naviPointCtrl.LoadItems;
    }
    private void RegisterEvents()
    {
        buildSelecter.onGetRootObjs += buildCtrl.SelectItem;
        naviPointSelecter.onGetRootObjs += naviPointCtrl.SelectItem;
        transGizmo.targetCtrl.onTransormingStateChanged += (x) =>
        {
            buildSelecter.SetEnable(!x);
        };
    }

    private void Update()
    {
        CameraHitUtility.Update(100);
        naviPointCtrl.Update();
        buildCtrl.Update();
       
    }
}
