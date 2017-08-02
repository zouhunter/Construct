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
    private VRLineRenderer lineRender;
    private const string lineShaderName = "VRLineRenderer/MeshChain - Alpha Blended";
    private float colorFlow;
    private Color startColor = Color.red;
    private Color endColor = Color.green;
    public NaviPointCtrl(NaviPoint prefab)
    {
        this.prefab = prefab;
        InitLineRender();
    }
    private void InitLineRender()
    {
        GameObject lineHolder = new GameObject("lineHolder");
        lineRender = lineHolder.AddComponent<VRLineRenderer>();
        var meshRender = lineHolder.GetComponent<MeshRenderer>();
        meshRender.material = new Material(Shader.Find(lineShaderName));
        meshRender.material.color = Color.blue;
        meshRender.material.SetColor("_Color", Color.white);
    }

    public void CreateItem()
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
                    pointList[i].name = pointList[i].Id.ToString();
                }
            }
        }
        else
        {
            item.Id = pointList.Count;
        }
        item.name = item.Id.ToString();
        ActiveTargetItem(item);
    }
    public void SelectItem(Transform[] arg0)
    {
        if (arg0 != null && arg0.Length > 0)
        {
            selectedItem = arg0[0].GetComponent<NaviPoint>();

            if (InputUtility.HaveClickMouseTwice(ref pickUpTimer,0,05f))
            {
                ActiveTargetItem(arg0[0].GetComponent<NaviPoint>());
            }
            else
            {
                //DoNothing
            }
        }
        else
        {
            ActiveTargetItem(null);
        }
    }

    public void LoadItems(NaviPoint[] arg0)
    {
        if (arg0 != null && arg0.Length > 0)
        {
            selectedItem = arg0[0];
            foreach (var item in pointList) {
                GameObject.Destroy(item.gameObject);
            }
            pointList.Clear();
            pointList.AddRange(arg0);
        }
    }
    public void Update()
    {
        if (selectedItem != null)
        {
            RefeshConnect();
        }
        if(activeItem !=null)
        {
            activeItem.BuildState = BuildState.inbuild;
            UpdateTargetPos();
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
                pointList[i].name = pointList[i].Id.ToString();
            }
            pointList.Remove(activeItem);
            GameObject.Destroy(activeItem.gameObject);
        }
    }
    private IEnumerator DelyPutDown()
    {
        yield return null;
        activeItem.BuildState = BuildState.normal;
        activeItem = null;
    }
    private void ActiveTargetItem(NaviPoint item)
    {
         activeItem = item;
    }
    /// <summary>
    /// 更新连接
    /// </summary>
    private void RefeshConnect()
    {
        pointList.Sort();
        lineRender.SetPositions(pointList.ConvertAll<Vector3>(x => x.transform.position).ToArray(),true);
        lineRender.SetWidth(1f, 1f);
        colorFlow += Time.deltaTime;
        if (colorFlow > 1)
        {
            colorFlow = 0;
        }
        startColor.r = colorFlow;
        endColor.g = 1 - colorFlow;
        lineRender.SetColors(startColor, endColor);
    }
}
