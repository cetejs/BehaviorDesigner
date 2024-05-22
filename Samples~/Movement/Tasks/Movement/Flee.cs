using UnityEngine;

namespace BehaviorDesigner.Tasks.Movement
{
    [TaskGroup("Movement")]
    [TaskDescription("Flee from the target specified using the Unity NavMesh.")]
    public class Flee : NavMeshMovement
    {
        [SerializeField]
        private SharedFloat fleeDistance = 20f;
        [SerializeField]
        private SharedFloat lookAheadDistance = 5f;
        [SerializeField]
        private SharedTransform target;
        private bool hasMoved;

        private Vector3 Target
        {
            get
            {
                Vector3 direction = transform.position - target.Value.position;
                if (direction.sqrMagnitude > 0.01f)
                {
                    direction.Normalize();
                }
                else
                {
                    direction = transform.forward;
                }

                return transform.position + direction * lookAheadDistance.Value;
            }
        }

        public override void OnStart()
        {
            base.OnStart();
            hasMoved = false;
            SetDestination(Target);
        }

        public override TaskStatus OnUpdate()
        {
            float sqrFleeDistance = fleeDistance.Value * fleeDistance.Value;
            if ((transform.position - target.Value.position).sqrMagnitude > sqrFleeDistance)
            {
                return TaskStatus.Success;
            }

            if (HasArrived)
            {
                if (!hasMoved)
                {
                    return TaskStatus.Failure;
                }

                if (!SetDestination(Target))
                {
                    return TaskStatus.Failure;
                }

                hasMoved = false;
            }
            else
            {
                float sqrSpeed = Velocity.sqrMagnitude;
                if (hasMoved && sqrSpeed <= 0f)
                {
                    return TaskStatus.Failure;
                }

                hasMoved = sqrSpeed > 0;
            }

            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            base.OnReset();
            fleeDistance = 20f;
            lookAheadDistance = 5f;
            target = null;
        }
    }
}