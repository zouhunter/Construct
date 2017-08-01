using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System;

public class NaviPoint : MonoBehaviour, ISelectable
{
    public Transform TransformComponent
    {
        get
        {
            return transform;
        }
    }
}
