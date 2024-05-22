using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityTransform
{
    [TaskGroup("Transform")]
    [TaskDescription("Stores the rotation of the Transform. Returns Success.")]
    public class GetRotation : Action
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedQuaternion storeResult;

        private Transform Target
        {
            get { return target.Value ? target.Value : transform; }
        }

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Target.rotation;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target = null;
            storeResult = Quaternion.identity;
        }
    }
}