using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityTransform
{
    [TaskGroup("Transform")]
    [TaskDescription("Performs comparison between the transformed distance and threshold: less than, less than or equal to, equal to, not equal to, greater than or equal to, or greater than.")]
    public class DistanceComparison : IntervalConditional
    {
        [SerializeField]
        private Operation operation;
        [SerializeField]
        private SharedTransform firstTransform;
        [SerializeField]
        private SharedTransform secondTransform;
        [SerializeField]
        private SharedFloat threshold = 10f;

        public override TaskStatus OnConditionalUpdate()
        {
            float sqrDistance = Vector3.SqrMagnitude(firstTransform.Value.position - secondTransform.Value.position);
            float sqrThreshold = threshold.Value * threshold.Value;
            switch (operation)
            {
                case Operation.LessThan:
                    return sqrDistance < sqrThreshold ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.LessThanOrEqualTo:
                    return sqrDistance <= sqrThreshold ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.EqualTo:
                    return Mathf.Approximately(sqrDistance, sqrThreshold) ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.NotEqualTo:
                    return !Mathf.Approximately(sqrDistance, sqrThreshold) ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.GraterThanOrEqualTo:
                    return sqrDistance >= sqrThreshold ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.GreaterThan:
                    return sqrDistance > sqrThreshold ? TaskStatus.Success : TaskStatus.Failure;
            }

            return TaskStatus.Failure;
        }

        public override void OnReset()
        {
            base.OnReset();
            operation = Operation.LessThan;
            firstTransform = null;
            secondTransform = null;
            threshold = 10f;
        }

        public enum Operation
        {
            LessThan,
            LessThanOrEqualTo,
            EqualTo,
            NotEqualTo,
            GraterThanOrEqualTo,
            GreaterThan
        }
    }
}