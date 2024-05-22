using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityMath
{
    [TaskGroup("Math")]
    [TaskDescription("Sets a bool value")]
    public class SetBool : Action
    {
        [SerializeField]
        private SharedBool boolValue;
        [SerializeField]
        private SharedBool storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = boolValue.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            boolValue = false;
            storeResult = false;
        }
    }
}