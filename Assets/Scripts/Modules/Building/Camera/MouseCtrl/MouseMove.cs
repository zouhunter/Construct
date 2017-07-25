using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class MouseMove  {
    [SerializeField]
    private Vector3 _viewRegion;
    //限制中心点
    [SerializeField]
    private Vector3 centerPos;
    //移动速度
    [SerializeField]
    private float _moveSpeed = 20;
    private Transform transform;
    public void Init(Transform transform)
    {
        this.transform = transform;
    }

    public void DirectionInputHandle(Vector3 forwardDir)
    {
        var forward = forwardDir * _moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.transform.SetPositionAndRotation(transform.position + forward, transform.rotation);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.SetPositionAndRotation(transform.position + Quaternion.Euler(Vector3.up * -90) * forward, transform.rotation);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.SetPositionAndRotation(transform.position - forward, transform.rotation);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.SetPositionAndRotation(transform.position + Quaternion.Euler(Vector3.up * 90) * forward, transform.rotation);
        }
    }
}
