namespace BehaviorDesigner.Tasks
{
    [TaskDescription("The sequence task is similar to an \"and\" operation. It will return failure as soon as one of its child tasks return failure. " +
                     "If a child task returns success then it will sequentially run the next task. If all child tasks return success then it will return success.")]
    public class Sequence : Composite
    {
        public override TaskStatus OnUpdate()
        {
            if (UpdateAbort())
            {
                if (CanExecute)
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

                    TaskStatus status = children[currentChildIndex].Update();
                    if (status == TaskStatus.Success || status == TaskStatus.Failure)
                    {
                        currentChildIndex++;
                        task.OnEnd();
                    }

                    if (status != TaskStatus.Success)
                    {
                        return status;
                    }
                }
            }

            return TaskStatus.Success;
        }
    }
}