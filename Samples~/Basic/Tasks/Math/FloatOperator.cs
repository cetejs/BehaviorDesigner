using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityMath
{
    [TaskGroup("Math")]
    [TaskDescription("Performs a math operation on two floats: Add, Subtract, Multiply, Divide, Modulo, Min, or Max.")]
    public class FloatOperation : Action
    {
        [SerializeField]
        private Operation operation;
        [SerializeField]
        private SharedFloat firstFloat;
        [SerializeField]
        private SharedFloat secondFloat;
        [SerializeField]
        private SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            switch (operation)
            {
                case Operation.Add:
                    storeResult.Value = firstFloat.Value + secondFloat.Value;
                    break;
                case Operation.Subtract:
                    storeResult.Value = firstFloat.Value - secondFloat.Value;
                    break;
                case Operation.Multiply:
                    storeResult.Value = firstFloat.Value * secondFloat.Value;
                    break;
                case Operation.Divide:
                    storeResult.Value = firstFloat.Value / secondFloat.Value;
                    break;
                case Operation.Modulo:
                    storeResult.Value = firstFloat.Value % secondFloat.Value;
                    break;
                case Operation.Min:
                    storeResult.Value = Mathf.Min(firstFloat.Value, secondFloat.Value);
                    break;
                case Operation.Max:
                    storeResult.Value = Mathf.Max(firstFloat.Value, secondFloat.Value);
                    break;
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            operation = Operation.Add;
            firstFloat = 0f;
            secondFloat = 0f;
            storeResult = 0f;
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