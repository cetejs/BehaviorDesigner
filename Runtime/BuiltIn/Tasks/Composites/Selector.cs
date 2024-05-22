namespace BehaviorDesigner
{
    [TaskIcon("Icons/SelectorIcon")]
    [TaskDescription("The selector task is similar to an \"or\" operation. It will return success as soon as one of its child tasks return success. " +
                     "If a child task returns failure then it will sequentially run the next task. If no child task returns success then it will return failure.")]
    public class Selector : Composite
    {
        public override TaskStatus OnUpdate()
        {
            if (UpdateAbort(true))
            {
                if (CanExecute && !children[currentChildIndex].IsDisabled)
                {
                    children[currentChildIndex].OnAbort();
                }

                RestartAbort();
            }

            while (CanExecute)
            {
                if (children[currentChildIndex].IsDisabled)
                {
                    currentChildIndex++;
                }
                else
                {
                    Task task = children[currentChildIndex];
                    if (CanChildStart)
                    {
                        task.OnStart();
                    }

                    TaskStatus status = children[currentChildIndex].OnUpdate(false);
                    if (status == TaskStatus.Success || status == TaskStatus.Failure)
                    {
                        currentChildIndex++;
                        task.OnEnd();
                    }

                    if (status == TaskStatus.Failure)
                    {
                        return TaskStatus.Running;
                    }

                    return status;
                }
            }

            return TaskStatus.Failure;
        }
    }
}