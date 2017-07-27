using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System;
[System.Serializable]
public class AutoFocusing {
    public float focusTime;
    private Transform transform;
    private bool isFocusing;
    internal void Init(Transform transform)
    {
        this.transform = transform;
    }

    public void Update(Vector3 target)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFocusing = true;
        }
        if (isFocusing)
        {
            transform.LookAt(target);
            isFocusing = false;
        }
    }
}
