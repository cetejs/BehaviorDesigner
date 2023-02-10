using UnityEngine;

namespace BehaviorDesigner.Tasks
{
    [TaskDescription("The repeater task will repeat execution of its child task until the child task has been run a specified number of times. " +
                     "It has the option of continuing to execute the child task even if the child task returns a failure.")]
    public class Repeater : Decorator
    {
        [SerializeField]
        private SharedInt count = 1;
        [SerializeField]
        private SharedBool isRepeatForever;
        [SerializeField]
        private SharedBool isEndOnFailure;

        private int executionCount;
        private TaskStatus executionStatus;

        public override TaskStatus OnDecorate(TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.Failure:
                    executionCount++;
                    if (isEndOnFailure.Value)
                    {
                        return status;
                    }
                    break;
                case TaskStatus.Success:
                    executionCount++;
                    break;
                case TaskStatus.Running:
                    return status;
            }
            
            if (isRepeatForever.Value || count.Value > executionCount)
            {
                lastChildIndex = -1;
                return TaskStatus.Running;
            }

            return status;
        }

        public override void OnReset()
        {
            count = 1;
            isRepeatForever = false;
            isEndOnFailure = false;
        }
    }
}