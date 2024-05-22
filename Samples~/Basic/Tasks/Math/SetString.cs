using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityMath
{
    [TaskGroup("Math")]
    [TaskDescription("Sets a string value")]
    public class SetString : Action
    {
        [SerializeField]
        private SharedString stringValue;
        [SerializeField]
        private SharedString storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = stringValue.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            stringValue = null;
            storeResult = null;
        }
    }
}