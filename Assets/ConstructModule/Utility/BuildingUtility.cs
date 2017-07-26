using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public static class BuildingUtility
{
    public static T GetComponentSecure<T>(this MonoBehaviour mb) where T:Component
    {
        return GetComponentSecure<T>(mb.gameObject);
    }
    public static T GetComponentSecure<T>(this GameObject go) where T : Component
    {
        T com = go.GetComponent<T>();
        if (com == null)
        {
            com = go.AddComponent<T>();
        }
        return com;
    }
}
