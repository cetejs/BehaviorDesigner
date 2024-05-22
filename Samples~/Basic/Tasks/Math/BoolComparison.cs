using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityMath
{
    [TaskGroup("Math")]
    [TaskDescription("Performs a comparison between two bools.")]
    public class BoolComparison : Conditional
    {
        [SerializeField]
        private SharedBool firstBool;
        [SerializeField]
        private SharedBool secondBool;

        public override TaskStatus OnUpdate()
        {
            return firstBool.Value == secondBool.Value ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            firstBool = false;
            secondBool = false;
        }
    }
}