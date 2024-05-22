using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityVector2
{
    [TaskGroup("Vector2")]
    [TaskName("Distance (V2)")]
    [TaskDescription("Returns the distance between two Vector2s.")]
    public class Distance : Action
    {
        [SerializeField]
        private SharedVector2 firstVector2;
        [SerializeField]
        private SharedVector2 secondVector2;
        [SerializeField]
        private SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Vector2.Distance(firstVector2.Value, secondVector2.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            firstVector2 = Vector2.zero;
            secondVector2 = Vector2.zero;
            storeResult = 0;
        }
    }
}