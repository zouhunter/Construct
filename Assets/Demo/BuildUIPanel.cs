using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public class BuildUIPanel : MonoBehaviour {
    public DragCamera dragCamera;
    public BuildingViewCamera buildCamera;

    private void OnGUI()
    {
        if (GUILayout.Button("Switch"))
        {
            dragCamera.SwitchisTopViewOr3D();
        }
        if (GUILayout.Button("相机切换"))
        {
            dragCamera.enabled = !dragCamera.enabled;
            buildCamera.enabled = !dragCamera.enabled;
        }
    }
}
