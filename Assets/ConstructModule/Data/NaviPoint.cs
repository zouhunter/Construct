using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System;

public class NaviPoint : MonoBehaviour, ISelectable,IComparable<NaviPoint>
{
    public int Id { get; set; }
    public Vector3[] quad { get; private set; }
    private BoxCollider _boxCollider;
    public Transform TransformComponent
    {
        get
        {
            return transform;
        }
    }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        UpdateQuad();
    }
    public void UpdateBuilding(Vector3 point)
    {
        transform.position = point;
    }
    /// <summary>
    /// 更新框体绘制信息
    /// </summary>
    public void UpdateQuad()
    {
        var colliderScale = BuildingUtility.ScaleAndCubeScale(transform.localScale, _boxCollider.size);
        quad = UpdateQuad(transform.position, transform.rotation, colliderScale.x * 1.2f, colliderScale.z * 1.2f);
    }

    private static Vector3[] UpdateQuad(Vector3 center, Quaternion rot, float wigth, float length)
    {
        var quad = new Vector3[4];
        quad[0] = center + rot * new Vector3(-wigth * 0.5f, 0.01f, length * 0.5f);
        quad[1] = center + rot * new Vector3(-wigth * 0.5f, 0.01f, -length * 0.5f);
        quad[2] = center + rot * new Vector3(wigth * 0.5f, 0.01f, -length * 0.5f);
        quad[3] = center + rot * new Vector3(wigth * 0.5f, 0.01f, length * 0.5f);
        return quad;
    }

    public int CompareTo(NaviPoint other)
    {
        if (Id > other.Id)
        {
            return 1;
        }
        else if(Id == other.Id)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }
}
