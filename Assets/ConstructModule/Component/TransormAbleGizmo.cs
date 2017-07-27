using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace RuntimeGizmos
{
    [RequireComponent(typeof(Camera))]
    public class TransormAbleGizmo : MonoBehaviour
    {
        private Transform[] targets;
        private Protocal protocal;
        private AxisSetting setting;
        private AxisDrawer drawer;
        private AxisOperator opertor;
        private UserSwitcher switcher;
        void Awake()
        {
            protocal = new Protocal();
            protocal.myCamera = GetComponent<Camera>();
            setting = new AxisSetting();
            switcher = new UserSwitcher(protocal, setting);
            drawer = new AxisDrawer(protocal, setting);
            opertor = new AxisOperator(this, protocal, setting);
        }

        void Update()
        {
            switcher.Update();
            opertor.Update();
        }

        private void LateUpdate()
        {
            opertor.LateUpdate();
            drawer.LateUpdate();
        }

        void OnPostRender()
        {
            drawer.OnPostRender();
        }

        #region BuildingUser
        public Transform SetTargets(params Transform[] targets)
        {
            if (targets == null || targets.Length == 0)
            {
                DeleteTarget();
                return null;
            }
            else
            {
                this.targets = targets;
                protocal.onTransormingStateChanged = onTransormingStateChanged;
                protocal.OnLocalScaleChanged = OnLocalScaleChanged;
                protocal.OnRotationChanged = OnRotationChanged;
                protocal.OnRotationChangedwithfloat = OnRotationChanged;
                protocal.OnPositionChanged = OnPositionChanged;
            }

            if (protocal.target == null)
            {
                protocal.target = new GameObject("SelectHolder").transform;
            }

            var pos = Vector3.zero;
            foreach (var item in targets)
            {
                pos += item.position;
            }

            protocal.target.position = pos / targets.Length;
            return protocal.target;
        }
        private void DeleteTarget()
        {
            if (protocal.target != null)
            {
                Destroy(protocal.target.gameObject);
            }
        }
        public UnityEngine.Events.UnityAction<bool> onTransormingStateChanged;
        public TransformSpace SwitchSpace()
        {
            if(protocal.space == TransformSpace.Global)
            {
                protocal.space = TransformSpace.Local;
            }
            else
            {
                protocal.space = TransformSpace.Global;
            }
            return protocal.space;
        }
        public void SetTransType(TransformType type)
        {
            if (protocal.type != type)
            {
                protocal.type = type;
            }
        }
        private void OnLocalScaleChanged(Vector3 scale)
        {
            if (targets != null)
                foreach (var item in targets)
                {
                    item.localScale += scale;
                }
        }
        private void OnRotationChanged(Vector3 rotation)
        {
            if (targets != null)
                foreach (var item in targets)
                {
                    item.Rotate(rotation * setting.allRotateSpeedMultiplier, Space.World);
                }
        }
        private void OnRotationChanged(Vector3 rotation, float rotateAmount)
        {
            if (targets != null)
                foreach (var item in targets)
                {
                    item.Rotate(rotation, rotateAmount, Space.World);
                }
        }
        private void OnPositionChanged(Vector3 value)
        {
            if (targets != null)
                foreach (var item in targets)
                {
                    item.Translate(value, Space.World);
                }
        }
        #endregion


    }
}