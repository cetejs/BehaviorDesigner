using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityTransform
{
    [TaskGroup("Transform")]
    [TaskDescription("Sets the local scale of the Transform. Returns Success.")]
    public class SetLocalScale : Action
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedVector3 localScale;

        private Transform Target
        {
            get { return target.Value ? target.Value : transform; }
        }

        public override TaskStatus OnUpdate()
        {
            Target.localScale = localScale.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target = null;
            localScale = Vector3.zero;
        }
    }
}