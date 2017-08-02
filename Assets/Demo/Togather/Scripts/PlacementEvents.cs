using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System.Reflection;
using System;

public class PlacementEvents
{
    public EventHold _eventHold = new EventHold();
    private static PlacementEvents _instence;
    public static PlacementEvents Instence
    {
        get
        {
            if (_instence == null)
            {
                _instence = new PlacementEvents();
            }
            return _instence;
        }
    }
    public class EventHold
    {
        public UnityEngine.Events.UnityAction<string> MessageNotHandled;
        public Dictionary<string, UnityAction<object>> m_needHandle = new Dictionary<string, UnityAction<object>>();
        public Dictionary<string, UnityAction> m_needHandle0 = new Dictionary<string, UnityAction>();
        public void NoMessageHandle(string rMessage)
        {
            if (MessageNotHandled == null)
            {
                Debug.LogWarning("MessageDispatcher: Unhandled Message of type " + rMessage);
            }
            else
            {
                MessageNotHandled(rMessage);
            }
        }

        #region 注册注销事件
        public void AddDelegate(string key, UnityAction<object> handle)
        {
            // First check if we know about the message type
            if (!m_needHandle.ContainsKey(key))
            {
                m_needHandle.Add(key, handle);
            }
            else
            {
                m_needHandle[key] += handle;
            }
        }
        public void AddDelegate(string key, UnityAction handle)
        {
            // First check if we know about the message type
            if (!m_needHandle0.ContainsKey(key))
            {
                m_needHandle0.Add(key, handle);
            }
            else
            {
                m_needHandle0[key] += handle;
            }
        }
        public bool RemoveDelegate(string key, UnityAction<object> handle)
        {
            if (m_needHandle.ContainsKey(key))
            {
                m_needHandle[key] -= handle;
                if (m_needHandle[key] == null)
                {
                    m_needHandle.Remove(key);
                    return false;
                }
            }
            return true;
        }
        public bool RemoveDelegate(string key, UnityAction handle)
        {
            if (m_needHandle0.ContainsKey(key))
            {
                m_needHandle0[key] -= handle;
                if (m_needHandle0[key] == null)
                {
                    m_needHandle0.Remove(key);
                    return false;
                }
            }
            return true;
        }
        public void RemoveDelegates(string key)
        {
            if (m_needHandle.ContainsKey(key))
            {
                m_needHandle.Remove(key);
            }
            if (m_needHandle0.ContainsKey(key))
            {
                m_needHandle0.Remove(key);
            }
        }
        #endregion

        #region 触发事件
        public void NotifyObserver(string key)
        {
            bool lReportMissingRecipient = true;

            if (m_needHandle0.ContainsKey(key))
            {
                m_needHandle0[key].Invoke();

                lReportMissingRecipient = false;
            }

            // If we were unable to send the message, we may need to report it
            if (lReportMissingRecipient)
            {
                NoMessageHandle(key);
            }
        }
        public void NotifyObserver<T>(string key, T value)
        {
            bool lReportMissingRecipient = true;

            if (m_needHandle.ContainsKey(key))
            {
                m_needHandle[key].Invoke(value);

                lReportMissingRecipient = false;
            }

            // If we were unable to send the message, we may need to report it
            if (lReportMissingRecipient)
            {
                NoMessageHandle(key);
            }
        }
        #endregion
    }

    public struct PosChangeData
    {
        public bool isX;
        public float value;
        public Action<bool> callBack;
    }
    public struct ItemActiveData
    {
        public float posx;
        public float posz;
        public float roty;
        public float scalex;
        public float viewAngle;
        public float viewDistence;
    }
    public struct ViewData
    {
        public float angle;
        public float distence;
    }
    public static string onItemActived_w;//对象被激活
    public static string onItemDisabled_w;//对象没有选中
    public static string onPositionChanged_w;//对象坐标发生改变
    public static string onRotateChanged_w;//对象旋转发生改变
    public static string onScaleChanged_w;//对象尺寸发生改变

    public static string onPosChanged_u;//界面数值坐标发生改变
    public static string onRotChanged_u;//界面数值旋转发生改变
    public static string onSizeChanged_u;//界面尺寸发生变化
    public static string onViewDistenceChanged_u;//观察点距离发生改变
    public static string onViewAngleChanged_u;//观察点角度发生改变

    public static string onCenterViewChanged;//视角中心变化 
    public static string onCreatePoint;//创建预览点

    static PlacementEvents()
    {
        var type = typeof(PlacementEvents);
        var fields = type.GetFields(BindingFlags.GetField | BindingFlags.Static | BindingFlags.Public);
        foreach (var item in fields)
        {
            item.SetValue(null, "TogatherEvents." + item.Name);
        }
    }
    public void Reset()
    {
        _eventHold.m_needHandle.Clear();
        _eventHold.m_needHandle0.Clear();
        _eventHold.MessageNotHandled = null;
    }
    public void RegisterEvent(string noti, UnityAction even)
    {
        _eventHold.AddDelegate(noti, even);
    }
    public void RegisterEvent<T>(string noti, UnityAction<object> even)
    {
        _eventHold.AddDelegate(noti, even);
    }

    public void RemoveEvent(string noti, UnityAction even)
    {
        _eventHold.RemoveDelegate(noti, even);
    }
    public void RemoveEvent<T>(string noti, UnityAction<object> even)
    {
        _eventHold.RemoveDelegate(noti, even);
    }

    public void RemoveEvents(string noti)
    {
        _eventHold.RemoveDelegates(noti);
    }

    public void InvokeEvents(string noti)
    {
        _eventHold.NotifyObserver(noti);
    }
    public void InvokeEvents<T>(string noti, T data)
    {
        _eventHold.NotifyObserver(noti, data);
    }
}
