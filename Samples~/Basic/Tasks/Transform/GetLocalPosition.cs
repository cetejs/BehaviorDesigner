﻿using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityTransform
{
    [TaskGroup("Transform")]
    [TaskDescription("Stores the local position of the Transform. Returns Success.")]
    public class GetLocalPosition : Action
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedVector3 storeResult;

        private Transform Target
        {
            get { return target.Value ? target.Value : transform; }
        }

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Target.localPosition;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target = null;
            storeResult = Vector3.zero;
        }
    }
}