using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public static class BuildingUtility
{
    public const string MovePlaneLayerName = "MovePos";
    public const string MoveItemLayerName = "MoveItem";
    public static Vector3 ScaleAndCubeScale(Vector3 scale,Vector3 cubeScale)
    {
        return new Vector3(scale.x * cubeScale.x, scale.y * cubeScale.y, scale.z * cubeScale.z);
    }
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
    public static bool IsPointInBox(Vector3 point, Vector3 centerPos, Vector3 size)
    {
        var halfBoxWeight = size.x * 0.5f;
        var halfBoxLength = size.z * 0.5f;
        var halfBoxHeight = size.y * 0.5f;
        var dir = point - centerPos;
        if (Mathf.Abs(dir.x) - halfBoxWeight < 0 && Mathf.Abs(dir.y) - halfBoxHeight < 0 && Mathf.Abs(dir.z) - halfBoxLength < 0)
        {
            return true;
        }
        return false;
    }

}
