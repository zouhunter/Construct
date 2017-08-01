using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System;
using RuntimeGizmos;
[System.Serializable]
public class NaviPointCtrl {
    private List<NaviPoint> pointList = new List<NaviPoint>();
    private NaviPoint activeItem;
    private RaycastHit hit;
    private float timer;
    private float lastDistence = 10;
    public NaviPoint prefab;
    public NaviPointSelectDrawer drawer;
    private LineRenderer lineRender;
    public NaviPointCtrl(NaviPoint prefab, NaviPointSelectDrawer drawer)
    {
        this.prefab = prefab;
        this.drawer = drawer;
        drawer.onGetRootObjs += SelectItem;
        InitLineRender();
    }
    private void InitLineRender()
    {
        GameObject lineHolder = new GameObject("lineHolder");
        lineRender = lineHolder.AddComponent<LineRenderer>();
        lineRender.material = new Material(Shader.Find("Unlit/Color"));
        lineRender.material.color = Color.blue;
        lineRender.startWidth = lineRender.endWidth = 0.1f;
        lineRender.positionCount = 0;
    }
    private void SelectItem(Transform[] arg0)
    {
        if (arg0 != null && arg0.Length > 0)
        {
            ActiveTargetItem(arg0[0].GetComponent<NaviPoint>());
        }
        else
        {
            ActiveTargetItem(null);
        }
    }
    public void Update()
    {
        if (activeItem != null)
        {
            UpdateTargetPos();
            RefeshConnect();
        }
    }
    private void UpdateTargetPos()
    {
        if (CameraHitUtility.GetOneHit(BuildingUtility.MovePlaneLayerName, ref hit))
        {
            activeItem.UpdateBuilding(hit.point);
            lastDistence = Vector3.Distance(Camera.main.transform.position, hit.point);
            if (InputUtility.HaveClickMouseTwice(ref timer, 0, 0.5f))
            {
                activeItem = null;
            }
        }
        else
        {
            activeItem.TransformComponent.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, lastDistence));
        }

        if (activeItem != null && InputUtility.HaveClickMouseTwice(ref timer, 1, 0.5f))
        {
            for (int i = activeItem.Id; i < pointList.Count; i++)
            {
                pointList[i].Id--;
            }
            pointList.Remove(activeItem);
            GameObject.Destroy(activeItem.gameObject);
        }
    }
    public void ActiveTargetItem(NaviPoint item)
    {
        activeItem = item;
    }
    internal void CreateItem()
    {
        var item = GameObject.Instantiate<NaviPoint>(prefab);
        item.Id = pointList.Count;
        pointList.Add(item);
        ActiveTargetItem(item.GetComponent<NaviPoint>());
    }

    /// <summary>
    /// 更新连接
    /// </summary>
    private void RefeshConnect()
    {
        lineRender.positionCount = pointList.Count;
        lineRender.SetPositions(pointList.ConvertAll<Vector3>(x => x.transform.position).ToArray());
    }
}
