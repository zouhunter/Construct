using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class BuildingItem : MonoBehaviour
{
    public BuildState state = BuildState.Normal;
    private BoxCollider _boxCollider;
    private RaycastHit[] hits;
    private Vector3 oldPos;
    public bool InstallAble;
    public Vector3[] quad = new Vector3[4];
    private Vector3 colliderScale;
    private void OnEnable()
    {
        _boxCollider = GetComponent<BoxCollider>();
        if (state == BuildState.Normal)
            SetBuildState(BuildState.Normal);
    }
    public void SetBuildState(BuildState state)
    {
        this.state = state;
    }

    public bool UpdatePos(Vector3 newPos)
    {
        transform.position = newPos;
        colliderScale = BuildingUtility.ScaleAndCubeScale(transform.localScale, _boxCollider.size);
        quad = UpdateQuad(transform.position,colliderScale.x * 1.2f,colliderScale.z * 1.2f);
        return JudePosition();
    }
    private static Vector3[] UpdateQuad(Vector3 center,float wigth,float length)
    {
        var quad = new Vector3[4];
        quad[0] = center + new Vector3(-wigth * 0.5f,0.01f, length * 0.5f );
        quad[1] = center + new Vector3(-wigth * 0.5f, 0.01f, -length * 0.5f );
        quad[2] = center + new Vector3(wigth * 0.5f, 0.01f, -length * 0.5f);
        quad[3] = center + new Vector3(wigth * 0.5f, 0.01f, length * 0.5f);
        return quad;
    }
    private bool JudePosition()
    {
        hits = Physics.BoxCastAll(
            _boxCollider.center + transform.position,
            colliderScale * 0.5f * 1.2f,
            Vector3.up,
            transform.rotation,
            10,
            LayerMask.GetMask(BuildingUtility.MoveItemLayerName));

        InstallAble = true;

        foreach (var item in hits)
        {
            if (item.collider.gameObject != gameObject)
            {
                InstallAble = false;
            }
        }
        return InstallAble;
    }
}

