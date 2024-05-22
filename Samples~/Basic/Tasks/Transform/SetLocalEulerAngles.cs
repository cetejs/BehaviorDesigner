using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityTransform
{
    [TaskGroup("Transform")]
    [TaskDescription("Sets the local euler angles of the Transform. Returns Success.")]
    public class SetLocalEulerAngles : Action
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedVector3 localEulerAngles;

        private Transform Target
        {
            get { return target.Value ? target.Value : transform; }
        }

        public override TaskStatus OnUpdate()
        {
            Target.localEulerAngles = localEulerAngles.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target = null;
            localEulerAngles = Vector3.zero;
        }
    }
}