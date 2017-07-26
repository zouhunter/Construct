using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class PosEvent: UnityEvent<Vector3>
{

}
public class SelectablePlane : MonoBehaviour
{
    public PosEvent onClickPos;

    private Transform targetObj;
    private Transform activeObj;
    private HitController HitCtrl;
    private Vector3 hitpos;
    private Collider m_Collider;

    public const string movePosTag = "MovePos";
    private void OnEnable()
    {
        HitCtrl = new HitController();
    }
    void Update()
    {
        HitCtrl.Update();

        if (targetObj == null)
        {
            SelectedTarget();
        }

        if (targetObj == null) return;

        if (Input.GetMouseButton(0))
        {
            UpdateTargetPos();
            if (Input.GetMouseButtonDown(0))
            {
                if (HitCtrl.GetHitPoint(ref hitpos))
                {
                    onClickPos.Invoke(hitpos);
                }
            }
        }
        else
        {
            targetObj.GetComponent<Collider>().enabled = true;
            targetObj = null;
        }
    }

    /// <summary>
    /// 选中对象
    /// </summary>
    void SelectedTarget()
    {
        if (HitCtrl.GetHitCollider(ref m_Collider) && !m_Collider.CompareTag(movePosTag))
        {
            if (m_Collider.transform.parent ==null ||( m_Collider.transform.parent != null && m_Collider.transform.parent.name == name))
            {
                targetObj = m_Collider.transform;
                targetObj.GetComponent<Collider>().enabled = false;
            }
        }
    }

    /// <summary>
    /// 更新所控制对象坐标
    /// </summary>
    void UpdateTargetPos()
    {
        if (HitCtrl.GetHitCollider(ref m_Collider) && m_Collider.CompareTag(movePosTag))
        {
            HitCtrl.GetHitPoint(ref hitpos);
            targetObj.transform.position = hitpos;
        }
    }
}

