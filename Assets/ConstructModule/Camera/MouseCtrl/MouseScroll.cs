using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System;

[System.Serializable]
public class MouseScroll {
    //x.y.z 方向上的移动限制
    //视角限制
    private Vector2 _FieldRegion;
    //滚轮缩放视角
    [SerializeField]
    private float _scrollSpeed;
    private Camera _camera;
    internal void Init(Camera _camera)
    {
        this._camera = _camera;
    }

    public void UpdateScroll()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            _camera.fieldOfView += _scrollSpeed * scroll * Time.deltaTime;
        }
    }
}
