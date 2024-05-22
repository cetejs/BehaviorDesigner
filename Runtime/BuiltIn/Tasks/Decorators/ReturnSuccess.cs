namespace BehaviorDesigner
{
    [TaskIcon("Icons/ReturnSuccessIcon")]
    [TaskDescription("The return success task will always return success except when the child task is running.")]
    public class ReturnSuccess : Decorator
    {
        public override TaskStatus OnDecorate(TaskStatus status)
        {
            if (status == TaskStatus.Failure)
            {
                return TaskStatus.Success;
            }

            return status;
        }
    }
}