using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityTransform
{
    [TaskGroup("Transform")]
    [TaskDescription("Rotates the transform so the forward vector points at worldPosition. Returns Success.")]
    public class LookAt : Action
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedTransform targetLookAt;
        [SerializeField]
        private SharedVector3 worldPosition;
        [SerializeField]
        private Vector3 worldUp;

        private Transform Target
        {
            get { return target.Value ? target.Value : transform; }
        }

        public override TaskStatus OnUpdate()
        {
            if (targetLookAt.Value)
            {
                Target.LookAt(targetLookAt.Value.transform);
            }
            else
            {
                Target.LookAt(worldPosition.Value, worldUp);
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            target = null;
            targetLookAt = null;
            worldPosition = Vector3.up;
            worldUp = Vector3.up;
        }
    }
}