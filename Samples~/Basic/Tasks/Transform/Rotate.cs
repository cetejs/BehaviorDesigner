using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityTransform
{
    [TaskGroup("Transform")]
    [TaskDescription("Applies a rotation. Returns Success.")]
    public class Rotate : Action
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedVector3 eulerAngles;
        [SerializeField]
        private Space relativeTo = Space.Self;

        private Transform Target
        {
            get { return target.Value ? target.Value : transform; }
        }

        public override TaskStatus OnUpdate()
        {
            Target.Rotate(eulerAngles.Value, relativeTo);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target = null;
            eulerAngles = Vector3.zero;
            relativeTo = Space.Self;
        }
    }
}