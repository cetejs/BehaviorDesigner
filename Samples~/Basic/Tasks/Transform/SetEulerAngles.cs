using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityTransform
{
    [TaskGroup("Transform")]
    [TaskDescription("Sets the euler angles of the Transform. Returns Success.")]
    public class SetEulerAngles : Action
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedVector3 eulerAngles;

        private Transform Target
        {
            get { return target.Value ? target.Value : transform; }
        }

        public override TaskStatus OnUpdate()
        {
            Target.eulerAngles = eulerAngles.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target = null;
            eulerAngles = Vector3.zero;
        }
    }
}