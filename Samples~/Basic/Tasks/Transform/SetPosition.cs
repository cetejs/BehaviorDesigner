using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityTransform
{
    [TaskGroup("Transform")]
    [TaskDescription("Sets the position of the Transform. Returns Success.")]
    public class SetPosition : Action
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedVector3 position;

        private Transform Target
        {
            get { return target.Value ? target.Value : transform; }
        }

        public override TaskStatus OnUpdate()
        {
            Target.localPosition = position.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target = null;
            position = Vector3.zero;
        }
    }
}