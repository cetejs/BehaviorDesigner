using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityTransform
{
    [TaskGroup("Transform")]
    [TaskDescription("Sets the local position of the Transform. Returns Success.")]
    public class SetLocalPosition : Action
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedVector3 localPosition;

        private Transform Target
        {
            get { return target.Value ? target.Value : transform; }
        }

        public override TaskStatus OnUpdate()
        {
            Target.localPosition = localPosition.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target = null;
            localPosition = Vector3.zero;
        }
    }
}