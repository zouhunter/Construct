using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
[System.Serializable]
public class PosEvent : UnityEvent<Vector3>
{

}
public class SelectablePlane : MonoBehaviour
{
    public NaviMeshHitCtrl HitCtrl;
    public UnityAction<bool> onMoveStateChanged;
    private BuildingItem targetObj;
    public const string movePosTag = "MovePos";
    public NavMeshSurface surface;
    private RaycastHit hit;
    private void OnEnable()
    {
        HitCtrl.Init();
    }
    void Update()
    {
        HitCtrl.Update(movePosTag);
        if (targetObj == null) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (onMoveStateChanged != null) onMoveStateChanged(true);
        }
        if (Input.GetMouseButton(0))
        {
             UpdateTargetPos();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (onMoveStateChanged != null) onMoveStateChanged(false);
            targetObj = null;
        }
    }
    public void SetTarget(BuildingItem target)
    {
        this.targetObj = target;
    }
    /// <summary>
    /// 更新所控制对象坐标
    /// </summary>
    void UpdateTargetPos()
    {
        if (HitCtrl.GetOneHitCollider(ref hit))
        {
            if (hit.collider != null)
            {
                targetObj.WorpToNewPos(hit.point);
            }
        }
    }
}

