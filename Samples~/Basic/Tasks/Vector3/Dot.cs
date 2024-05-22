using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityVector3
{
    [TaskGroup("Vector3")]
    [TaskName("Dot (V3)")]
    [TaskDescription("Stores the dot product of two Vector3 values.")]
    public class Dot : Action
    {
        [SerializeField]
        private SharedVector3 leftHandSide;
        [SerializeField]
        private SharedVector3 rightHandSide;
        [SerializeField]
        private SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Vector3.Dot(leftHandSide.Value, rightHandSide.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            leftHandSide = Vector3.zero;
            rightHandSide = Vector3.zero;
            storeResult = 0;
        }
    }
}