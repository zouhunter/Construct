using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace RuntimeGizmos
{
    [RequireComponent(typeof(Camera))]
    public class GizmoBehaviour : MonoBehaviour
    {
        public AxisSetting setting;
        private AxisDrawer drawer;
        private AxisOperator opertor;
        private AxisSwitcher switcher;
        public TargetCtrl targetCtrl;
        void Awake()
        {
            var protocal = new Protocal(GetComponent<Camera>());
            switcher = new AxisSwitcher(protocal, setting);
            drawer = new AxisDrawer(protocal, setting);
            opertor = new AxisOperator(this, protocal, setting);
            targetCtrl = new TargetCtrl(protocal, setting);
        }

        void Update()
        {
            switcher.QuickSwitchAndSelect();
            opertor.TryTransformSelected();
        }

        private void LateUpdate()
        {
            opertor.TrySetAxisInfo();
            drawer.TrySetLines();
        }

        void OnPostRender()
        {
            drawer.TryDrawing();
        }
    }
}