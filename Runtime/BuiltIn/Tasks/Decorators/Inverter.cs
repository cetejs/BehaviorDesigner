namespace BehaviorDesigner
{
    [TaskIcon("Icons/InverterIcon")]
    [TaskDescription("The inverter task will invert the return value of the child task after it has finished executing. " +
                     "If the child returns success, the inverter task will return failure. If the child returns failure, the inverter task will return success.")]
    public class Inverter : Decorator
    {
        public override TaskStatus OnDecorate(TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.Success:
                    return TaskStatus.Failure;
                case TaskStatus.Failure:
                    return TaskStatus.Success;
            }

            return status;
        }
    }
}