using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityTransform
{
    [TaskGroup("Transform")]
    [TaskDescription("Applies a rotation. Returns Success.")]
    public class RotateAround : Action
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedVector3 point;
        [SerializeField]
        private SharedVector3 axis;
        [SerializeField]
        private SharedFloat angle;

        private Transform Target
        {
            get { return target.Value ? target.Value : transform; }
        }

        public override TaskStatus OnUpdate()
        {
            Target.RotateAround(point.Value, axis.Value, angle.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target = null;
            point = Vector3.zero;
            axis = Vector3.zero;
            angle = 0f;
        }
    }
}