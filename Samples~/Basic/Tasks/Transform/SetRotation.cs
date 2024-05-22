using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityTransform
{
    [TaskGroup("Transform")]
    [TaskDescription("Sets the rotation of the Transform. Returns Success.")]
    public class SetRotation : Action
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedQuaternion rotation;

        private Transform Target
        {
            get { return target.Value ? target.Value : transform; }
        }

        public override TaskStatus OnUpdate()
        {
            Target.localRotation = rotation.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target = null;
            rotation = Quaternion.identity;
        }
    }
}