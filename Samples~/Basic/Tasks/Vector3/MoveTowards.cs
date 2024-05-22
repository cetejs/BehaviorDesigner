using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityVector3
{
    [TaskGroup("Vector3")]
    [TaskName("MoveTowards (V3)")]
    [TaskDescription("Move from the current position to the target position.")]
    public class MoveTowards : Action
    {
        [SerializeField]
        private SharedVector3 currentPosition;
        [SerializeField]
        private SharedVector3 targetPosition;
        [SerializeField]
        private SharedFloat speed;
        [SerializeField]
        private SharedVector3 storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Vector3.MoveTowards(currentPosition.Value, targetPosition.Value, speed.Value * Time.deltaTime);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            currentPosition = Vector3.zero; 
            targetPosition = Vector3.zero;
            storeResult = Vector3.zero;
            speed = 0;
        }
    }
}