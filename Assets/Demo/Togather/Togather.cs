using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using RuntimeGizmos;

public class Togather : SceneMain<Togather>
{
    private NaviPointCtrl naviPointCtrl;
    [SerializeField]
    private NaviPoint naviPoint;
    [SerializeField]
    private NaviPointSelectDrawer selectDrawer;
    private void Start()
    {
        naviPointCtrl = new NaviPointCtrl(naviPoint,selectDrawer);
        RegisterEvent(TogatherEvents.onCreatePoint, naviPointCtrl.CreateItem);
    }
    private void Update()
    {
        naviPointCtrl.Update();
    }
}
