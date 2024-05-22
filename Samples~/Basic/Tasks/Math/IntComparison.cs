using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityMath
{
    [TaskGroup("Math")]
    [TaskDescription("Performs comparison between two integers: less than, less than or equal to, equal to, not equal to, greater than or equal to, or greater than.")]
    public class IntComparison : Conditional
    {
        [SerializeField]
        private Operation operation;
        [SerializeField]
        private SharedInt firstInt;
        [SerializeField]
        private SharedInt secondInt;

        public override TaskStatus OnUpdate()
        {
            switch (operation)
            {
                case Operation.LessThan:
                    return firstInt.Value < secondInt.Value ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.LessThanOrEqualTo:
                    return firstInt.Value <= secondInt.Value ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.EqualTo:
                    return firstInt.Value == secondInt.Value ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.NotEqualTo:
                    return firstInt.Value != secondInt.Value ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.GraterThanOrEqualTo:
                    return firstInt.Value >= secondInt.Value ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.GreaterThan:
                    return firstInt.Value > secondInt.Value ? TaskStatus.Success : TaskStatus.Failure;
            }

            return TaskStatus.Failure;
        }

        public override void OnReset()
        {
            operation = Operation.LessThan;
            firstInt = 0;
            secondInt = 0;
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