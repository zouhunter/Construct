using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.AI;
using System;
[System.Serializable]
public class CameraHitUtility
{
    private static Ray ray;
    private static RaycastHit[] hits;
    private static bool hited;

    public static void Update(float distence)
    {
        if (Camera.main)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray, distence);
            if (hits != null && hits.Length > 0)
            {
                hited = true;
            }
            else
            {
                hited = false;
            }
        }
        else
        {
            hited = false;
        }
    }
    public static bool GetOneHit(string layerName,ref RaycastHit outHit)
    {
        if (hited && hits!= null && hits.Length > 0)
        {
            foreach (var item in hits)
            {
                if (item.collider.gameObject.layer == LayerMask.NameToLayer(layerName))
                {
                    outHit = item;
                    return true;
                }
            }
            return false;
        }
        return false;
    }
    public static bool GetHits(string layerName, ref List<RaycastHit> outHits)
    {
        if (hited && hits != null && hits.Length > 0)
        {
            outHits.Clear();
            foreach (var item in hits)
            {
                if (item.collider.gameObject.layer == LayerMask.NameToLayer(layerName))
                {
                    outHits.Add(item);
                }
            }
            return false;
        }
        return false;
    }
}
