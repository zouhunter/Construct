using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class DragCamera : MonoBehaviour
{
    private Camera _camera;
    [SerializeField]
    private bool _isTopView;

    [SerializeField]
    private MouseRotate _mouseRotate;
    [SerializeField]
    private MouseMove _mouseMove;
    [SerializeField]
    private MouseScroll _mouseScroll;

    private Quaternion lastQuater;
    private Vector3 lastPosition;
    public bool IsTopView { get { return _isTopView; } }
    private void Start()
    {
        _camera = GetComponent<Camera>();
        _mouseRotate.Init(transform);
        _mouseMove.Init(transform);
        _mouseScroll.Init(_camera);
    }
    void Update()
    {
        _mouseMove.DirectionInputHandle(GetForwardWithSpeed());
        _mouseScroll.UpdateScroll();
        if (_isTopView)
        {

        }
        else
        {
            _mouseRotate.UpdateLookRotation();
        }
    }
    /// <summary>
    /// 切换顶视面或3维视图
    /// </summary>
    public void SwitchisTopViewOr3D(Vector3 centerPos = default(Vector3))
    {
        var position = centerPos == default(Vector3) ? transform.position : new Vector3(centerPos.x, transform.position.y, centerPos.z);
        _isTopView = !_isTopView;
        if (_isTopView)
        {
            lastPosition = transform.position;
            lastQuater = transform.rotation;
            transform.localEulerAngles = new Vector3(90, 0, 0);
            transform.position = position;
        }
        else
        {
            transform.position = lastPosition;
            transform.rotation = lastQuater;
        }
    }
  
    private Vector3 GetForwardWithSpeed()
    {
        Vector3 foward = Vector3.zero;
        if (_isTopView)
        {
            foward = Vector3.forward;
        }
        else
        {
            foward = transform.forward;
            foward.y = 0;
        }
        return foward ;
    }
}
