using UnityEngine;

namespace BehaviorDesigner.Tasks.Movement
{
    [TaskGroup("Movement")]
    [TaskDescription("Move towards the specified position. The position can either be specified by a transform or position. If the transform " +
                     "is used then the position will not be used.")]
    public class MoveTowards : Action
    {
        [SerializeField]
        private SharedFloat speed = 10f;
        [SerializeField]
        private SharedFloat angularSpeed = 120f;
        [SerializeField]
        private SharedFloat arriveDistance = 0.1f;
        [SerializeField]
        private SharedBool lookAtTarget = true;
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

        public override TaskStatus OnUpdate()
        {
            Vector3 direction = Target - transform.position;
            float sqrDistance = arriveDistance.Value * arriveDistance.Value;
            if (direction.sqrMagnitude <= sqrDistance)
            {
                return TaskStatus.Success;
            }

            transform.position = Vector3.MoveTowards(transform.position, Target, speed.Value * Time.deltaTime);
            if (lookAtTarget.Value && direction.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction),  angularSpeed.Value * Time.deltaTime);
            }

            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            speed = 10f;
            angularSpeed = 120f;
            arriveDistance = 0.1f;
            lookAtTarget = true;
            target = null;
            targetPosition = Vector3.zero;
        }
    }
}