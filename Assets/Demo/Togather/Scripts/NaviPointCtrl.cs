using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System;
using RuntimeGizmos;
[System.Serializable]
public class NaviPointCtrl
{
    private List<NaviPoint> pointList = new List<NaviPoint>();
    private NaviPoint activeItem;
    private NaviPoint selectedItem;
    private RaycastHit hit;
    private float pickDownTimer;
    private float pickUpTimer;
    private float lastDistence = 10;
    public NaviPoint prefab;
    private LineRenderer lineRender;
    public NaviPointCtrl(NaviPoint prefab)
    {
        this.prefab = prefab;
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
    public void SelectItem(Transform[] arg0)
    {
        if (arg0 != null && arg0.Length > 0)
        {
            if (InputUtility.HaveClickMouseTwice(ref pickUpTimer,0,05f))
            {
                ActiveTargetItem(arg0[0].GetComponent<NaviPoint>());
            }
            else
            {
                selectedItem = arg0[0].GetComponent<NaviPoint>();
            }
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
            if (InputUtility.HaveClickMouseTwice(ref pickDownTimer, 0, 0.5f))
            {
                activeItem.StartCoroutine(DelyPutDown());
            }
        }
        else
        {
            activeItem.TransformComponent.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, lastDistence));
        }

        if (activeItem != null && InputUtility.HaveClickMouseTwice(ref pickDownTimer, 1, 0.5f))
        {
            for (int i = activeItem.Id; i < pointList.Count; i++)
            {
                pointList[i].Id--;
            }
            pointList.Remove(activeItem);
            GameObject.Destroy(activeItem.gameObject);
        }
    }

    IEnumerator DelyPutDown()
    {
        yield return null;
        activeItem = null;
    }
    public void ActiveTargetItem(NaviPoint item)
    {
         activeItem = item;
    }
    internal void CreateItem()
    {
        var item = GameObject.Instantiate<NaviPoint>(prefab);
        pointList.Add(item);
        if (selectedItem != null)
        {
            item.Id = selectedItem.Id;
            if (pointList.Count > selectedItem.Id)
            {
                for (int i = selectedItem.Id; i < pointList.Count; i++)
                {
                    pointList[i].Id++;
                    pointList[i].name = pointList[i].Id.ToString() ;
                }
            }
        }
        else
        {
            item.Id = pointList.Count;
        }
        item.name = item.Id.ToString();
        ActiveTargetItem(item.GetComponent<NaviPoint>());
    }

    /// <summary>
    /// 更新连接
    /// </summary>
    private void RefeshConnect()
    {
        lineRender.positionCount = pointList.Count;
        pointList.Sort();
        lineRender.SetPositions(pointList.ConvertAll<Vector3>(x => x.transform.position).ToArray());
    }
}
