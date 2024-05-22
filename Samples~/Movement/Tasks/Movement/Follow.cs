using UnityEngine;

namespace BehaviorDesigner.Tasks.Movement
{
    [TaskGroup("Movement")]
    [TaskDescription("Follows the specified target using the Unity NavMesh.")]
    public class Follow : NavMeshMovement
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedFloat moveDistance = 1f;

        private Vector3 lastTargetPosition;

        public override void OnStart()
        {
            base.OnStart();
            lastTargetPosition = target.Value.position + Vector3.one * moveDistance.Value;
        }

        public override TaskStatus OnUpdate()
        {
            float sqrMoveDistance = moveDistance.Value * moveDistance.Value;
            Vector3 targetPosition = target.Value.position;
            if ((targetPosition - lastTargetPosition).sqrMagnitude > sqrMoveDistance)
            {
                SetDestination(targetPosition);
                lastTargetPosition = targetPosition;
            }
            else if ((targetPosition - transform.position).sqrMagnitude < sqrMoveDistance)
            {
                Stop();
                lastTargetPosition = targetPosition;
            }

            return TaskStatus.Running;
        }
        
        public override void OnReset()
        {
            base.OnReset();
            target = null;
            moveDistance = 1f;
        }
    }
}