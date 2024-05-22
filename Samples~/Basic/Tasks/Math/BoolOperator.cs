using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityMath
{
    [TaskGroup("Math")]
    [TaskDescription("Performs a math operation on two bools: AND, OR, NAND, or XOR.")]
    public class BoolOperator : Action
    {
        [SerializeField]
        private Operation operation;
        [SerializeField]
        private SharedBool firstBool;
        [SerializeField]
        private SharedBool secondBool;
        [SerializeField]
        private SharedBool storeResult;

        public override TaskStatus OnUpdate()
        {
            switch (operation)
            {
                case Operation.AND:
                    storeResult.Value = firstBool.Value && secondBool.Value;
                    break;
                case Operation.OR:
                    storeResult.Value = firstBool.Value || secondBool.Value;
                    break;
                case Operation.NAND:
                    storeResult.Value = !(firstBool.Value && secondBool.Value);
                    break;
                case Operation.XOR:
                    storeResult.Value = firstBool.Value ^ secondBool.Value;
                    break;
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            operation = Operation.AND;
            firstBool = false;
            secondBool = false;
            storeResult = false;
        }

        public enum Operation
        {
            AND,
            OR,
            NAND,
            XOR,
        }
    }
}