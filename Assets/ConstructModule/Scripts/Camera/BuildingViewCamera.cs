﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
/// <summary>
/// 点击状态
/// </summary>
public enum ClickType {
    nil = 0,
    leftDown = 1,//左键
    centerDown = 1<<1,
    centerScroll = 1<<2,
    rightDown = 1<<3
}
public class BuildingViewCamera : MonoBehaviour
{
    [Header("中心")]
    public Transform target;
    private Vector3 cameraTarget {
        get { return target.position; }
    }

    [Header("速度")]
    public float mouseWheelSenstitivity = 100f;
    public float xSpeed = 250f;
    public float ySpeed = 150f;
    public float viewChangeSpeed = 1f;
    public float targetMoveSpeed = 20f;
    [Header("限制")]
    public float mouseZoomMin = 0.6f;
    public float mouseZoomMax = 200f;
    public float yMinLimit = -10f;
    public float yMaxLimit = 200f;
    public float normalDistance = 40f;
    public float viewMin = 2;
    public float viewMax = 30;
    [Header("变化")]
    public float xdlt = 0f;
    public float ydlt = 0f;
    //private float ycrr = 0f;
    public float ndlt = 0f;

    [Header("目标")]
    public Vector3 targetRotate;
    public Vector3 targetPosition;
    public float targetView;
    [Header("调控")]
    public bool isNeedDamping = true;
    public float damping = 10f;
    public bool isDisableMove;
   

    private ClickType clickType;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Camera lookCamera;
#if !NoFunction
    private Vector3 targettargetPos;
#endif
    private float dotweenduration
    {
        get { return 1; }
    }
    void OnEnable(){
        lookCamera = GetComponentInChildren<Camera>();
        targetView = lookCamera.fieldOfView;
        SetSelf(transform);
    }

    public void SetSelf(Transform _self)
	{
#if !NoFunction
        normalDistance = Vector3.Distance(targettargetPos, _self.position);
        targetRotate = _self.rotation.eulerAngles;
		targetPosition = _self.position;
#endif
	}
    public void SetTarget(Transform _target)
    {
#if !NoFunction
        if (_target == null) return;
        targettargetPos = _target.position;
        normalDistance = Vector3.Distance(transform.position, targettargetPos);
        target.position = targettargetPos;
#endif
    }

    void Update()
    {
        if (target != null)
        {
            ChangeClickType();
            StoreChangeData();
            TargetControll();
        }
    }
    void LateUpdate()
    {
        if (target!=null&&!isDisableMove)
        {
            ExecuteCameraChange();
            transform.LookAt(target);
        }
    }
    /// <summary>
    /// 改变当前的点击状态
    /// </summary>
    private void ChangeClickType()
    {
        clickType = ClickType.nil;
        if (Input.GetMouseButton(0))
        {
            clickType |= ClickType.leftDown;
        }
        if (Input.GetMouseButton(1))
        {
            clickType |= ClickType.rightDown;
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            clickType |= ClickType.centerScroll;
        }
        if (Input.GetMouseButton(2))
        {
            clickType |= ClickType.centerDown;
        }
    }
    /// <summary>
    /// 存储要改变的信息
    /// </summary>
    private void StoreChangeData()
    {
        xdlt = ydlt = ndlt = 0f;

        //单击左键(记录xy方向的变化量)
        if ((clickType & ClickType.leftDown) == ClickType.leftDown)
        {
            //xdlt = Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            //ydlt = -Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        }
        //单击右键（加速）
        if (clickType == ClickType.rightDown)
        {
            xdlt += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            ydlt += -Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        }
        //滑动中键（记录距离）
        if ((clickType & ClickType.centerScroll) == ClickType.centerScroll)
        {
            if (normalDistance >= mouseZoomMin && normalDistance <= mouseZoomMax)
                ndlt = Input.GetAxis("Mouse ScrollWheel") * mouseWheelSenstitivity * 0.02f;
        }
        //右键加中键（改变视角大小）
        if (((clickType & ClickType.centerScroll) == ClickType.centerScroll) && (clickType & ClickType.rightDown) == ClickType.rightDown)
        {
            targetView += ndlt * mouseWheelSenstitivity * viewChangeSpeed * 0.02f;
            targetView = Mathf.Clamp(targetView, viewMin, viewMax);
            return;
        }
        //目标旋转量和坐标
        normalDistance -= ndlt * mouseWheelSenstitivity;
        normalDistance = Mathf.Clamp(normalDistance, mouseZoomMin, mouseZoomMax);

        targetRotate = targetRotate + new Vector3(ydlt, xdlt, 0);
        targetPosition = Quaternion.Euler(targetRotate) * new Vector3(0f, 0f, -normalDistance) + cameraTarget;
        targetPosition = ClampPositon(targetPosition,yMinLimit,yMaxLimit);
    }
    /// <summary>
    /// 利用得到的变量设置相机的新
    /// </summary>
    private void ExecuteCameraChange()
    {
        if (isNeedDamping)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotate), Time.deltaTime * damping);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * damping);
            lookCamera.fieldOfView = Mathf.Lerp(lookCamera.fieldOfView,targetView, Time.deltaTime * damping);
        }
        else
        {
            transform.rotation = Quaternion.Euler(targetRotate);
            transform.position = targetPosition;
            lookCamera.fieldOfView = targetView;
        }
    }
    /// <summary>
    /// 限定角度的范围
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private Vector3 ClampPositon(Vector3 pos, float min, float max)
    {
        pos.y = Mathf.Clamp(pos.y, min, max);
        return pos;
    }

    private Vector3 targetMove;
    
    private void TargetControll()
    {
        if (targetMoveSpeed == 0) return;
        targetMove.x = Input.GetAxis("Horizontal") * targetMoveSpeed * Time.deltaTime;
        targetMove.z = Input.GetAxis("Vertical") * targetMoveSpeed * Time.deltaTime;
        targetMove = transform.rotation * targetMove + target.transform.position;
        targetMove.y = target.transform.position.y;
        target.transform.position = targetMove;
    }
}

