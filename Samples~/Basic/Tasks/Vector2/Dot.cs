using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityVector2
{
    [TaskGroup("Vector2")]
    [TaskName("Dot (V2)")]
    [TaskDescription("Stores the dot product of two Vector2 values.")]
    public class Dot : Action
    {
        [SerializeField]
        private SharedVector2 leftHandSide;
        [SerializeField]
        private SharedVector2 rightHandSide;
        [SerializeField]
        private SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Vector2.Dot(leftHandSide.Value, rightHandSide.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            leftHandSide = Vector2.zero;
            rightHandSide = Vector2.zero;
            storeResult = 0;
        }
    }
}