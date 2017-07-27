using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.AI;
using System;
[System.Serializable]
public class NaviMeshHitCtrl
{
    private Ray ray;
    private RaycastHit[] hits;
    private bool hited;

    [SerializeField]
    private float distence;
    public void Init()
    {

    }

    public void Update(string layerName)
    {
        if (Camera.main)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray, distence, LayerMask.GetMask(layerName));
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
    public bool GetOneHitCollider(ref RaycastHit hits)
    {
        if (hited && this. hits!= null && this.hits.Length > 0)
        {
            hits = this.hits[0];
            return true;
        }
        return false;
    }
}
