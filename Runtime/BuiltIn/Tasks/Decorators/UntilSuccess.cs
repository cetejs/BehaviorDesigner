namespace BehaviorDesigner
{
    [TaskIcon("Icons/UntilSuccessIcon")]
    [TaskDescription("The until success task will keep executing its child task until the child task returns success.")]
    public class UntilSuccess : Decorator
    {
        public override TaskStatus OnDecorate(TaskStatus status)
        {
            if (status == TaskStatus.Failure)
            {
                lastChildIndex = -1;
                return TaskStatus.Running;
            }

            return status;
        }
    }
}