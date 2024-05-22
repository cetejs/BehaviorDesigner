using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityTransform
{
    [TaskGroup("Transform")]
    [TaskDescription("Sets the local rotation of the Transform. Returns Success.")]
    public class SetLocalRotation : Action
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedQuaternion localRotation;

        private Transform Target
        {
            get { return target.Value ? target.Value : transform; }
        }

        public override TaskStatus OnUpdate()
        {
            Target.localRotation = localRotation.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target = null;
            localRotation = Quaternion.identity;
        }
    }
}