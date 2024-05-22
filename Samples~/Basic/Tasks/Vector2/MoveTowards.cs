using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityVector2
{
    [TaskGroup("Vector2")]
    [TaskName("MoveTowards (V2)")]
    [TaskDescription("Move from the current position to the target position.")]
    public class MoveTowards : Action
    {
        [SerializeField]
        private SharedVector2 currentPosition;
        [SerializeField]
        private SharedVector2 targetPosition;
        [SerializeField]
        private SharedFloat speed;
        [SerializeField]
        private SharedVector2 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Vector2.MoveTowards(currentPosition.Value, targetPosition.Value, speed.Value * Time.deltaTime);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            currentPosition = Vector2.zero; 
            targetPosition = Vector2.zero;
            storeResult = Vector2.zero;
            speed = 0;
        }
    }
}