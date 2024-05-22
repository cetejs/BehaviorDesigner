using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityVector3
{
    [TaskGroup("Vector3")]
    [TaskName("Lerp (V3)")]
    [TaskDescription("Lerp the Vector3 by an amount.")]
    public class Lerp : Action
    {
        [SerializeField]
        private SharedVector3 fromVector3;
        [SerializeField]
        private SharedVector3 toVector3;
        [SerializeField]
        private SharedFloat lerpAmount;
        [SerializeField]
        private SharedVector3 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Vector3.Lerp(fromVector3.Value, toVector3.Value, lerpAmount.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            fromVector3 = Vector3.zero;
            toVector3 = Vector3.zero;
            storeResult = Vector3.zero;
            lerpAmount = 0;
        }
    }
}