using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System.Reflection;
using System;

public static class TogatherEvents {
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
    static TogatherEvents()
    {
        var type = typeof(TogatherEvents);
        var fields = type.GetFields(BindingFlags.GetField | BindingFlags.Static | BindingFlags.Public);
        foreach (var item in fields){
            item.SetValue(null, "TogatherEvents." + item.Name);
        }
    }
}
