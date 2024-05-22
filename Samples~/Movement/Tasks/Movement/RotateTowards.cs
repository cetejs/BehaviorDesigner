using UnityEngine;

namespace BehaviorDesigner.Tasks.Movement
{
    [TaskGroup("Movement")]
    [TaskDescription("Rotates towards the specified rotation. The rotation can either be specified by a transform or rotation. If the transform "+
                     "is used then the rotation will not be used.")]
    public class RotateTowards : Action
    {
        [SerializeField]
        private SharedFloat speed = 120f;
        [SerializeField]
        private SharedFloat rotationEpsilon = 0.1f;
        [SerializeField]
        private SharedBool onlyY;
        [SerializeField]
        private SharedTransform target;
        [SerializeField]
        private SharedVector3 targetRotation;

        private Quaternion Target
        {
            get
            {
                if (target.Value)
                {
                    Vector3 direction = target.Value.position - transform.position;
                    if (direction.sqrMagnitude < 0.01f)
                    {
                        return transform.rotation;
                    }

                    if (onlyY.Value)
                    {
                        direction.y = 0f;
                    }

                    return Quaternion.LookRotation(direction);
                }

                return Quaternion.Euler(targetRotation.Value);
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (Quaternion.Angle(transform.rotation, Target) < rotationEpsilon.Value)
            {
                return TaskStatus.Success;
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Target, speed.Value * Time.deltaTime);
            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            speed = 120f;
            rotationEpsilon = 0.1f;
            onlyY = false;
            target = null;
            targetRotation = Vector3.zero;
        }
    }
}