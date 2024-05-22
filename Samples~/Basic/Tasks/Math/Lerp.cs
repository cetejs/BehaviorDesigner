using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityMath
{
    [TaskGroup("Math")]
    [TaskDescription("Lerp the float by an amount.")]
    public class Lerp : Action
    {
        [SerializeField]
        private SharedFloat fromValue;
        [SerializeField]
        private SharedFloat toValue;
        [SerializeField]
        private SharedFloat lerpAmount;
        [SerializeField]
        private SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Mathf.Lerp(fromValue.Value, toValue.Value, lerpAmount.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            fromValue = 0;
            toValue = 0;
            lerpAmount = 0;
            storeResult = 0;
        }
    }
}