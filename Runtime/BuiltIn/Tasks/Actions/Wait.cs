using UnityEngine;

namespace BehaviorDesigner
{
    [TaskIcon("Icons/WaitIcon")]
    [TaskDescription("Wait a specified amount of time. The task will return running until the task is done waiting. It will return success after the wait time has elapsed.")]
    public class Wait : Action
    {
        [SerializeField]
        private SharedFloat waitTime = 1f;
        [SerializeField]
        private SharedBool isRandomWait;
        [SerializeField]
        private SharedVector2 randomRange = new Vector2(0f, 1f);

        private float startTime;
        private float waitDuration;

        public override void OnStart()
        {
            base.OnStart();
            startTime = Time.time;
            if (isRandomWait.Value)
            {
                waitDuration = Random.Range(randomRange.Value.x, randomRange.Value.y);
            }
            else
            {
                waitDuration = waitTime.Value;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (Time.time - startTime > waitDuration)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            waitTime = 1f;
            isRandomWait = false;
            randomRange = new Vector2(0f, 1f);
        }
    }
}