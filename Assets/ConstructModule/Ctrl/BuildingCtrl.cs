using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using ListView;
using System;
using RuntimeGizmos;

[System.Serializable]
public class BuildingCtrl
{
    private BuildingItem _activeItem;
    public BuildingItem ActiveItem { get { return _activeItem; } }
    public UnityAction<BuildingItem> onBuildOK;
    private BuildingItem _selectedItem;
    private RaycastHit hit;
    private float putDownTimer;
    private float pickuptimer;
    private float lastDistence = 10;
    private Vector3 lastTargetPos;
    private GizmoBehaviour transGizmo;
    private DragCamera dragCamera;
    public BuildingCtrl(GizmoBehaviour transGizmo, DragCamera dragCamera)
    {
        this.transGizmo = transGizmo;
        this.dragCamera = dragCamera;
        RegiserEventOfBuildItem();
        RegisterEventFromUIPanel();
    }

    public void Update()
    {
        if (ActiveItem != null)
        {
            OperateActiveItem();
        }
        if (_selectedItem != null && _selectedItem.isActiveAndEnabled)
        {
            _selectedItem.UpdateQuad();
        }
    }

    public void SelectItem(Transform[] arg0)
    {
        if (ActiveItem != null) return;

        if (arg0 != null && arg0.Length > 0)
        {
            OnTransFormItemActive(arg0[0]);
            if (InputUtility.HaveExecuteTwicePerSecond(ref pickuptimer, 0.5f))
            {
                OnGizmosItems(null);
                var item = arg0[0].GetComponent<BuildingItem>();
                if (item.buildState == BuildState.normal)
                    SetTargetItemSafe(item);
            }
            else
            {
                OnGizmosItems(arg0);
            }

        }
        else
        {
            PlacementEvents.Instence.InvokeEvents(PlacementEvents.onItemDisabled_w);
            SetTargetItemSafe(null);
            OnGizmosItems(null);
        }
    }
    public void CreateBuildItem(BuildItemHold hold)
    {
        var item = GameObject.Instantiate(hold.prefab).GetComponent<BuildingItem>();
        item.deviceName = hold.itemName;
        UnDoUtility.RecordStep(new CreateStepRecord(item));
        SetTargetItemSafe(item);
    }
    #region Private Method
    private void OperateActiveItem()
    {
        if (CameraHitUtility.GetOneHit(BuildingUtility.MovePlaneLayerName, ref hit))
        {
            ActiveItem.UpdateBuilding(hit.point);
            lastDistence = Vector3.Distance(Camera.main.transform.position, hit.point);
            if (ActiveItem.quadInfo.installAble && InputUtility.HaveClickMouseTwice(ref putDownTimer, 0, 0.5f))
            {
                ActiveItem.StartCoroutine(DelyPutDown());
            }
        }
        else
        {
            ActiveItem.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, lastDistence));
        }

        if (InputUtility.HaveClickMouseTwice(ref putDownTimer, 1, 0.5f))
        {
            BuildingItem item = ActiveItem.GetComponent<BuildingItem>();
            UnDoUtility.RecordStep(new DestroyStepRecord(item));
            //RemoveBuilding(item);
            _activeItem = null;
            if (onBuildOK != null)
            {
                onBuildOK(null);
            }
        }
    }
    /// <summary>
    /// 延时放下（防止又拿起）
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelyPutDown()
    {
        yield return null;
        BuildingItem item = ActiveItem.GetComponent<BuildingItem>();
        item.buildState = BuildState.normal;
        UnDoUtility.RecordStep(new TransformStepRecord(ActiveItem));
        SetTargetItemSafe(null);
        if (onBuildOK != null)
        {
            onBuildOK(item);
        }
    }
    /// <summary>
    /// 激活对象
    /// </summary>
    /// <param name="item"></param>
    private void SetTargetItemSafe(BuildingItem item)
    {
        //正在建造中无法选择其他对象
        if (ActiveItem != null && ActiveItem.buildState == BuildState.inbuild)
        {
            return;
        }
        else
        {
            _activeItem = item;
            if (ActiveItem != null)
            {
                ActiveItem.buildState = BuildState.inbuild;
                ActiveItem.onPositionChanged = (x) =>
                {
                    PlacementEvents.Instence.InvokeEvents<Vector3>(PlacementEvents.onPositionChanged_w, x);
                };
            }
        }

    }
    private void RegiserEventOfBuildItem()
    {
        transGizmo.targetCtrl.onRotationChanged += () =>
        {
            if (_selectedItem != null)
            {
                PlacementEvents.Instence.InvokeEvents(PlacementEvents.onRotateChanged_w, _selectedItem.transform.eulerAngles.y);
            }
        };
        transGizmo.targetCtrl.onLocalScaleChanged += () =>
        {
            if (_selectedItem != null)
            {
                PlacementEvents.Instence.InvokeEvents(PlacementEvents.onScaleChanged_w, _selectedItem.transform.localScale.x);
            }
        };
    }
    /// <summary>
    /// 注册来自于其他界面的事件
    /// </summary>
    private void RegisterEventFromUIPanel()
    {
        //注册坐标发生改变
        PlacementEvents.Instence.RegisterEvent<PlacementEvents.PosChangeData>(PlacementEvents.onPosChanged_u, (UnityAction<object>)(x =>
        {
            var data = (PlacementEvents.PosChangeData)x;
            if (this._selectedItem != null)
            {
                var newPos = this._selectedItem.transform.position;
                if (data.isX)
                {
                    newPos.x = data.value;
                }
                else
                {
                    newPos.z = data.value;
                }
                var reseted = this._selectedItem.ResetPosition((Vector3)newPos);
                data.callBack.Invoke((bool)reseted);
            }
            else
            {
                data.callBack.Invoke(false);
            }
        }));
        //注册旋转发生改变
        PlacementEvents.Instence.RegisterEvent<float>(PlacementEvents.onRotChanged_u, (UnityAction<object>)(x =>
        {
            if (this._selectedItem != null)
            {
                var angle = this._selectedItem.transform.eulerAngles;
                angle.y = (float)x;
                this._selectedItem.transform.eulerAngles = angle;
            }

        }));
        //注册尺寸发生改变
        PlacementEvents.Instence.RegisterEvent<float>(PlacementEvents.onSizeChanged_u, (UnityAction<object>)(x =>
        {
            if (this._selectedItem != null)
            {
                var angle = this._selectedItem.transform.localScale;
                if (angle.x == 0)
                {
                    angle = new Vector3(1, 1, 1);
                }
                var plus = ((float)x) / angle.x;
                angle *= plus;
                this._selectedItem.transform.localScale = angle;
            }

        }));
        //观察点距离发生改变
        PlacementEvents.Instence.RegisterEvent<float>(PlacementEvents.onViewDistenceChanged_u, (UnityAction<object>)(x =>
        {
            if (this._selectedItem != null)
            {
                this._selectedItem.SetViewDistence((float)x);
            }
        }));
        //观察点距离发生改变
        PlacementEvents.Instence.RegisterEvent<float>(PlacementEvents.onViewAngleChanged_u, (UnityAction<object>)(x =>
        {
            if (this._selectedItem != null)
            {
                this._selectedItem.SetViewAngle((float)x);
            }
        }));
    }
    private void OnGizmosItems(Transform[] trans)
    {
        if (trans == null || trans.Length == 0)
        {
            transGizmo.targetCtrl.SetTargets(EnableState.Clamp, null);
        }
        else
        {
            _selectedItem = trans[0].GetComponent<BuildingItem>();
            var root = transGizmo.targetCtrl.SetTargets(EnableState.Clamp, _selectedItem.transform);
            if (root != null)
            {
                lastTargetPos = root.position;
                dragCamera.SetTarget(lastTargetPos);
                PlacementEvents.Instence.InvokeEvents<Vector3>(PlacementEvents.onCenterViewChanged, lastTargetPos);

            }
        }

    }
    /// <summary>
    /// 点击到某个对象
    /// </summary>
    /// <param name="item"></param>
    private void OnTransFormItemActive(Transform item)
    {
        PlacementEvents.ItemActiveData data = new global::PlacementEvents.ItemActiveData();
        data.posx = item.position.x;
        data.posz = item.position.z;
        data.roty = item.eulerAngles.y;
        data.scalex = item.localScale.x;
         _selectedItem = item.GetComponent<BuildingItem>();
        _selectedItem.GetViewPos(out data.viewAngle, out data.viewDistence);
        PlacementEvents.Instence.InvokeEvents<PlacementEvents.ItemActiveData>(PlacementEvents.onItemActived_w, data);
    }
    #endregion
}

