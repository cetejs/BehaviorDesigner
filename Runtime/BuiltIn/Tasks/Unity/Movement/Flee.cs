using UnityEngine;

namespace BehaviorDesigner.Tasks.Movement
{
    [TaskDescription("Flee from the target specified using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    public class Flee : NavMeshMovement
    {
        [SerializeField]
        private SharedFloat fleeDistance = 20f;
        [SerializeField]
        private SharedFloat lookAheadDistance = 5f;
        [SerializeField]
        protected SharedTransform target;

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
                if (!SetDestination(Target))
                {
                    return TaskStatus.Failure;
                }
            }
            else if (Velocity.sqrMagnitude == 0f)
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Running;
        }

        protected override bool SetDestination(Vector3 target)
        {
            if (!SamplePosition(target))
            {
                return false;
            }

            return base.SetDestination(target);
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