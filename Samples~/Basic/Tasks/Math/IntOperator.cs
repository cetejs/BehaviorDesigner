using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityMath
{
    [TaskGroup("Math")]
    [TaskDescription("Performs a math operation on two integers: Add, Subtract, Multiply, Divide, Modulo, Min, or Max.")]
    public class IntOperation : Action
    {
        [SerializeField]
        private Operation operation;
        [SerializeField]
        private SharedInt firstInt;
        [SerializeField]
        private SharedInt secondInt;
        [SerializeField]
        private SharedInt storeResult;

        public override TaskStatus OnUpdate()
        {
            switch (operation)
            {
                case Operation.Add:
                    storeResult.Value = firstInt.Value + secondInt.Value;
                    break;
                case Operation.Subtract:
                    storeResult.Value = firstInt.Value - secondInt.Value;
                    break;
                case Operation.Multiply:
                    storeResult.Value = firstInt.Value * secondInt.Value;
                    break;
                case Operation.Divide:
                    storeResult.Value = firstInt.Value / secondInt.Value;
                    break;
                case Operation.Modulo:
                    storeResult.Value = firstInt.Value % secondInt.Value;
                    break;
                case Operation.Min:
                    storeResult.Value = Mathf.Min(firstInt.Value, secondInt.Value);
                    break;
                case Operation.Max:
                    storeResult.Value = Mathf.Max(firstInt.Value, secondInt.Value);
                    break;
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            operation = Operation.Add;
            firstInt = 0;
            secondInt = 0;
            storeResult = 0;
        }

        public enum Operation
        {
            Add,
            Subtract,
            Multiply,
            Divide,
            Modulo,
            Min,
            Max,
        }
    }
}