using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityVector2
{
    [TaskGroup("Vector2")]
    [TaskName("Lerp (V2)")]
    [TaskDescription("Lerp the Vector2 by an amount.")]
    public class Lerp : Action
    {
        [SerializeField]
        private SharedVector2 fromVector2;
        [SerializeField]
        private SharedVector2 toVector2;
        [SerializeField]
        private SharedFloat lerpAmount;
        [SerializeField]
        private SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Vector2.Lerp(fromVector2.Value, toVector2.Value, lerpAmount.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            fromVector2 = Vector2.zero;
            toVector2 = Vector2.zero;
            storeResult = Vector2.zero;
            lerpAmount = 0;
        }
    }
}