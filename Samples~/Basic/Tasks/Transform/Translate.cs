using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityTransform
{
    [TaskGroup("Transform")]
    [TaskDescription("Moves the transform in the direction and distance of translation. Returns Success.")]
    public class Translate : Action
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedVector3 translation;
        [SerializeField]
        private Space relativeTo = Space.Self;

        private Transform Target
        {
            get { return target.Value ? target.Value : transform; }
        }

        public override TaskStatus OnUpdate()
        {
            Target.Translate(translation.Value, relativeTo);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target = null;
            translation = Vector3.zero;
            relativeTo = Space.Self;
        }
    }
}