using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using ListView;
using System;
using RuntimeGizmos;

public class LocalListPanel : MonoBehaviour
{
    public BuildingCtrl buildCtrl;
    private BuildingItem activeItem;
    private BuildItemSelectDrawer quadDrawer;
    private GizmoBehaviour transGizmo;
    private DragCamera dragCamera;
    private Vector3 lastTargetPos;
    private float timer = 0;
    void Start()
    {
        quadDrawer = Camera.main.GetComponent<BuildItemSelectDrawer>();
        transGizmo = Camera.main.GetComponent<GizmoBehaviour>();
        dragCamera = Camera.main.GetComponent<DragCamera>();

        buildCtrl.Init();
        quadDrawer.onGetRootObjs += SelectItem;
        transGizmo.targetCtrl.onTransormingStateChanged = (x) =>
        {
            quadDrawer.enabled = !x;
        };

        RegiserEventOfBuildItem();
        RegisterEventFromUIPanel();
    }
    private void RegiserEventOfBuildItem()
    {
        transGizmo.targetCtrl.onRotationChanged = () =>
        {
            if (activeItem != null)
                SceneMain.Current.InvokeEvents(TogatherEvents.onRotateChanged_w, activeItem.transform.eulerAngles.y);
        };
        transGizmo.targetCtrl.onLocalScaleChanged = () =>
        {
            if (activeItem != null)
                SceneMain.Current.InvokeEvents(TogatherEvents.onScaleChanged_w, activeItem.transform.localScale.x);
        };
    }
    /// <summary>
    /// 注册来自于其他界面的事件
    /// </summary>
    private void RegisterEventFromUIPanel()
    {
        //注册坐标发生改变
        SceneMain.Current.RegisterEvent<TogatherEvents.PosChangeData>(TogatherEvents.onPosChanged_u, x =>
        {
            var data = (TogatherEvents.PosChangeData)x;
            if (activeItem != null)
            {
                var newPos = activeItem.transform.position;
                if (data.isX)
                {
                    newPos.x = data.value;
                }
                else
                {
                    newPos.z = data.value;
                }
                var reseted = activeItem.ResetPosition(newPos);
                data.callBack.Invoke(reseted);
            }
            else
            {
                data.callBack.Invoke(false);
            }
        });
        //注册旋转发生改变
        SceneMain.Current.RegisterEvent<float>(TogatherEvents.onRotChanged_u, x =>
        {
            if (activeItem != null)
            {
                var angle = activeItem.transform.eulerAngles;
                angle.y = (float)x;
                activeItem.transform.eulerAngles = angle;
            }

        });
        //注册尺寸发生改变
        SceneMain.Current.RegisterEvent<float>(TogatherEvents.onSizeChanged_u, x =>
        {
            if (activeItem != null)
            {
                var angle = activeItem.transform.localScale;
                var plus = ((float)x) / angle.x;
                angle *= plus;
                activeItem.transform.localScale = angle;
            }

        });
        //观察点距离发生改变
        SceneMain.Current.RegisterEvent<float>(TogatherEvents.onViewDistenceChanged_u, x =>
        {
            if (activeItem != null)
            {
                activeItem.SetViewDistence((float)x);
            }
        });
        //观察点距离发生改变
        SceneMain.Current.RegisterEvent<float>(TogatherEvents.onViewAngleChanged_u, x =>
        {
            if (activeItem != null)
            {
                activeItem.SetViewAngle((float)x);
            }
        });
    }

    private void SelectItem(Transform[] arg0)
    {
        if (arg0 != null && arg0.Length > 0)
        {
            OnActiveItem(arg0[0]);
            //防止放下的瞬间又拿起
            if (Array.Find(arg0, x => x.GetComponent<BuildingItem>() == buildCtrl.ActiveItem) != null)
            {
                return;
            }

            if (InputUtility.HaveExecuteTwicePerSecond(ref timer, 0.5f))
            {
                OnGizmosItems(null);
                foreach (var bitem in arg0)
                {
                    var item = bitem.GetComponent<BuildingItem>();
                    if (item.buildState == BuildState.normal)
                        buildCtrl.ActiveTargetItem(item);
                }
            }
            else
            {
                OnGizmosItems(arg0);
            }

        }
        else
        {
            SceneMain.Current.InvokeEvents(TogatherEvents.onItemDisabled_w);
            activeItem = null;
            buildCtrl.ActiveTargetItem(null);
            OnGizmosItems(null);
        }
    }
    private void OnGizmosItems(Transform[] trans)
    {
        if (trans == null || trans.Length == 0)
        {
            transGizmo.targetCtrl.SetTargets(null);
        }
        else
        {
            activeItem = trans[0].GetComponent<BuildingItem>();

            //lastTrans = x;
            var root = transGizmo.targetCtrl.SetTargets(new BuildingItem[] { activeItem });
            if (root != null)
            {
                lastTargetPos = root.position;
                dragCamera.SetTarget(lastTargetPos);
            }
        }

    }
    private void OnActiveItem(Transform item)
    {
        TogatherEvents.ItemActiveData data = new global::TogatherEvents.ItemActiveData();
        data.posx = item.position.x;
        data.posz = item.position.z;
        data.roty = item.eulerAngles.y;
        data.scalex = item.localScale.x;
        var bitem = item.GetComponent<BuildingItem>();
        bitem.GetViewPos(out data.viewAngle, out data.viewDistence);

        SceneMain.Current.InvokeEvents<TogatherEvents.ItemActiveData>(TogatherEvents.onItemActived_w, data);

    }
    private void Update()
    {
        CameraHitUtility.Update(100);
        buildCtrl.Update();
        if (activeItem != null)
        {
            if (activeItem != null && activeItem.isActiveAndEnabled)
            {
                activeItem.UpdateQuad();
            }
        }
    }

}
