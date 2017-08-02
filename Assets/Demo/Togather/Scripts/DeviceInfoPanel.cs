using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class DeviceInfoPanel : MonoBehaviour
{
    [SerializeField]//世界坐标x
    private InputField m_px;
    private string m_pxValue;

    [SerializeField]//世界坐标y
    private InputField m_py;
    private string m_pyValue;

    [SerializeField]//旋转值y
    private InputField m_ry;

    [SerializeField]//比例值
    private InputField m_scale;

    [SerializeField]//观察点距离
    private InputField m_dP;

    [SerializeField]//观察点旋转
    private InputField m_rP;

    //可触发不可点击状态
    private CanvasGroup m_CanvasGroup;
    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_CanvasGroup.interactable = false;
        RegisterWorldItemChange();
        RegiserUIChangeEvents();
    }
    private void RegisterWorldItemChange()
    {
        PlacementEvents.Instence.RegisterEvent(PlacementEvents.onItemDisabled_w, OnItemDisabled);
        PlacementEvents.Instence.RegisterEvent<Vector3>(PlacementEvents.onPositionChanged_w, OnPosxChanged);
        PlacementEvents.Instence.RegisterEvent<PlacementEvents.ItemActiveData>(PlacementEvents.onItemActived_w, OnItemActived);
        PlacementEvents.Instence.RegisterEvent<float>(PlacementEvents.onRotateChanged_w, OnRotateChanged);
        PlacementEvents.Instence.RegisterEvent<float>(PlacementEvents.onScaleChanged_w, OnScaleChanged);
    }
    private void RegiserUIChangeEvents()
    {
        m_px.onEndEdit.AddListener((x) => OnPosChanged(true, x));
        m_py.onEndEdit.AddListener((x) => OnPosChanged(false, x));
        m_ry.onEndEdit.AddListener(x=> { PlacementEvents.Instence.InvokeEvents(PlacementEvents.onRotChanged_u, float.Parse(x)); });
        m_scale.onEndEdit.AddListener(x => { PlacementEvents.Instence.InvokeEvents(PlacementEvents.onSizeChanged_u, float.Parse(x)); });
        m_dP.onEndEdit.AddListener(x => { PlacementEvents.Instence.InvokeEvents(PlacementEvents.onViewDistenceChanged_u, float.Parse(x)); });
        m_rP.onEndEdit.AddListener(x=> { PlacementEvents.Instence.InvokeEvents(PlacementEvents.onViewAngleChanged_u, float.Parse(x)); });
    }
    private void OnPosChanged(bool isX,string x)
    {
        PlacementEvents.PosChangeData data = new global::PlacementEvents.PosChangeData();
        data.isX = isX;
        data.value = x==""?0: float.Parse(x);
        data.callBack = new System.Action<bool>((isOK) => {
            if(isOK)
            {
                if (isX)
                {
                    m_pxValue = m_px.text;
                }
                else
                {
                    m_pyValue = m_py.text;
                }
            }
            else
            {
                if (isX)
                {
                    m_px.text = m_pxValue;
                }
                else
                {
                    m_py.text = m_pyValue;
                }
            }
        });
        PlacementEvents.Instence.InvokeEvents(PlacementEvents.onPosChanged_u, data);
    }
    private void OnPosxChanged(object x)
    {
        var vec = (Vector3)x;
        m_pxValue = m_px.text = vec.x.ToString("0.00");
        m_pyValue = m_py.text = vec.z.ToString("0.00");
    }
    private void OnItemDisabled()
    {
        m_CanvasGroup.interactable = false;
    }
    private void OnItemActived(object obj)
    {
        m_CanvasGroup.interactable = true;

        var trans = (PlacementEvents.ItemActiveData)obj;
        m_pxValue = m_px.text = trans.posx.ToString("0.00");
        m_pyValue = m_py.text = trans.posz.ToString("0.00");
        m_ry.text = trans.roty.ToString("0.00");
        m_scale.text = trans.scalex.ToString("0.00");
        m_dP.text = trans.viewDistence.ToString("0.00");
        m_rP.text = trans.viewAngle.ToString("0.00");
    }
    private void OnRotateChanged(object y)
    {
        var ry = (float)y;
        m_ry.text = ry.ToString("0.00");
    }
    private void OnScaleChanged(object x)
    {
        var rsize = (float)x;
        m_scale.text = rsize.ToString("0.00");
    }
    private void OnDestroy()
    {
        PlacementEvents.Instence.RemoveEvent<Transform>(PlacementEvents.onItemActived_w, OnItemActived);
        PlacementEvents.Instence.RemoveEvent<Vector3>(PlacementEvents.onPositionChanged_w, OnPosxChanged);
    }
}
