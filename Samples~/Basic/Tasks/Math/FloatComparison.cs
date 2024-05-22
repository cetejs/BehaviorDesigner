using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityMath
{
    [TaskGroup("Math")]
    [TaskDescription("Performs comparison between two floats: less than, less than or equal to, equal to, not equal to, greater than or equal to, or greater than.")]
    public class FloatComparison : Conditional
    {
        [SerializeField]
        private Operation operation;
        [SerializeField]
        private SharedFloat firstFloat;
        [SerializeField]
        private SharedFloat secondFloat;

        public override TaskStatus OnUpdate()
        {
            switch (operation)
            {
                case Operation.LessThan:
                    return firstFloat.Value < secondFloat.Value ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.LessThanOrEqualTo:
                    return firstFloat.Value <= secondFloat.Value ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.EqualTo:
                    return Mathf.Approximately(firstFloat.Value, secondFloat.Value) ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.NotEqualTo:
                    return !Mathf.Approximately(firstFloat.Value, secondFloat.Value) ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.GraterThanOrEqualTo:
                    return firstFloat.Value >= secondFloat.Value ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.GreaterThan:
                    return firstFloat.Value > secondFloat.Value ? TaskStatus.Success : TaskStatus.Failure;
            }

            return TaskStatus.Failure;
        }

        public override void OnReset()
        {
            operation = Operation.LessThan;
            firstFloat = 0f;
            secondFloat = 0f;
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