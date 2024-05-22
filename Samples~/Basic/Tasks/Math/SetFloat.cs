using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityMath
{
    [TaskGroup("Math")]
    [TaskDescription("Sets a float value")]
    public class SetFloat : Action
    {
        [SerializeField]
        private SharedFloat floatValue;
        [SerializeField]
        private SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = floatValue.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            floatValue = 0f;
            storeResult = 0f;
        }
    }
}