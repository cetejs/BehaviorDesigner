using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityMath
{
    [TaskCategory("Math")]
    [TaskDescription("Sets a int value")]
    public class SetInt : Action
    {
        [SerializeField]
        private SharedInt intValue;
        [SerializeField] [RequiredField]
        private SharedInt storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = intValue.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            intValue = 0;
            storeResult = 0;
        }
    }
}