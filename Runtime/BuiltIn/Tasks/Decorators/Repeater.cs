using UnityEngine;

namespace BehaviorDesigner
{
    [TaskIcon("Icons/RepeaterIcon")]
    [TaskDescription("The repeater task will repeat execution of its child task until the child task has been run a specified number of times. " +
                     "It has the option of continuing to execute the child task even if the child task returns a failure.")]
    public class Repeater : Decorator
    {
        [SerializeField]
        private SharedInt count = 1;
        [SerializeField]
        private SharedBool repeatForever;
        [SerializeField]
        private SharedBool endOnFailure;

        private int executionCount;
        private TaskStatus executionStatus;

        public override TaskStatus OnDecorate(TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.Failure:
                    executionCount++;
                    if (endOnFailure.Value)
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

            if (repeatForever.Value || count.Value > executionCount)
            {
                lastChildIndex = -1;
                return TaskStatus.Running;
            }

            return status;
        }

        public override void OnReset()
        {
            count = 1;
            repeatForever = false;
            endOnFailure = false;
        }
    }
}