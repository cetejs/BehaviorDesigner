using UnityEngine;

namespace BehaviorDesigner.Tasks.Movement
{
    [TaskGroup("Movement")]
    [TaskDescription("Seek the target specified using the Unity NavMesh.")]
    public class Seek : NavMeshMovement
    {
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedVector3 targetPosition;

        private Vector3 Target
        {
            get
            {
                if (target.Value)
                {
                    return target.Value.position;
                }

                return targetPosition.Value;
            }
        }

        public override void OnStart()
        {
            base.OnStart();
            SetDestination(Target);
        }

        public override TaskStatus OnUpdate()
        {
            if (HasArrived)
            {
                return TaskStatus.Success;
            }

            SetDestination(Target);
            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            base.OnReset();
            target = null;
            targetPosition = Vector3.zero;
        }
    }
}