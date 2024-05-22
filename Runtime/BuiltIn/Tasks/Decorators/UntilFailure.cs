namespace BehaviorDesigner
{
    [TaskIcon("Icons/UntilFailureIcon")]
    [TaskDescription("The until failure task will keep executing its child task until the child task returns failure.")]
    public class UntilFailure : Decorator
    {
        public override TaskStatus OnDecorate(TaskStatus status)
        {
            if (status == TaskStatus.Success)
            {
                lastChildIndex = -1;
                return TaskStatus.Running;
            }

            return status;
        }
    }
}