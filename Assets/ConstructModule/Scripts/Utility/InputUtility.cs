using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using UnityEngine.Events;
using System.IO;

public static class InputUtility {

    /// <summary>
    /// 在Update中调用判断是否进行了双击
    /// </summary>
    /// <returns></returns>
    public static bool HaveClickMouseTwice(ref float Timer,int keyNum, float time = 1)
    {
        if (Input.GetMouseButtonDown(keyNum))
        {
            return HaveExecuteTwicePerSecond(ref Timer, time);
        }
        return false;
    }
    /// <summary>
    /// 是否在指定的时间执行了两次
    /// </summary>
    /// <param name="offsetTime"></param>
    /// <param name="currentTime"></param>
    /// <param name="timer"></param>
    /// <param name="executeOnce"></param>
    /// <returns></returns>
    public static bool HaveExecuteTwicePerSecond(ref float timer, float time = 1)
    {
        if (Time.time - timer < time)
        {
            return true;
        }
        else
        {
            timer = Time.time;
            return false;
        }
    }
    /// <summary>
    /// 是否点击到了UI界面
    /// </summary>
    /// <returns></returns>
    public static bool HaveClickUI()
    {
        if (UnityEngine.EventSystems.EventSystem.current !=null && 
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }

}
