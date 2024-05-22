using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityVector3
{
    [TaskGroup("Vector3")]
    [TaskName("Cross (V3)")]
    [TaskDescription("Stores the cross of two Vector3 values.")]
    public class Cross : Action
    {
        [SerializeField]
        private SharedVector3 leftHandSide;
        [SerializeField]
        private SharedVector3 rightHandSide;
        [SerializeField]
        private SharedVector3 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Vector3.Cross(leftHandSide.Value, rightHandSide.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            leftHandSide = Vector3.zero;
            rightHandSide = Vector3.zero;
            storeResult = Vector3.zero;
        }
    }
}