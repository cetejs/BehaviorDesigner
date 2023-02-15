using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityMath
{
    [TaskCategory("Math")]
    [TaskDescription("Sets a string value")]
    public class SetString : Action
    {
        [SerializeField]
        private SharedString stringValue;
        [SerializeField] [RequiredField]
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