using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[Serializable]
public class MouseRotate
{
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;

    private Transform camera;
    public void Init(Transform camera)
    {
        this.camera = camera;
    }

    public void UpdateLookRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            camera.localEulerAngles += new Vector3(-xRot, yRot, 0f);
        }
    }
}
