namespace BehaviorDesigner
{
    [TaskIcon("Icons/ReturnFailureIcon")]
    [TaskDescription("The return failure task will always return failure except when the child task is running.")]
    public class ReturnFailure : Decorator
    {
        public override TaskStatus OnDecorate(TaskStatus status)
        {
            if (status == TaskStatus.Success)
            {
                return TaskStatus.Failure;
            }

            return status;
        }
    }
}