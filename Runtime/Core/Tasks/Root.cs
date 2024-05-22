namespace BehaviorDesigner
{
    [TaskDescription("The root node of the behavior tree cannot be deleted.")]
    public sealed class Root : ParentTask
    {
        public override int MaxChildren
        {
            get { return 1; }
        }

        public override TaskStatus OnUpdate()
        {
            if (CanExecute)
            {
                Task child = children[currentChildIndex];
                if (!child.IsDisabled)
                {
                    if (CanChildStart)
                    {
                        child.OnStart();
                    }

                    TaskStatus status = child.OnUpdate(false);
                    if (status == TaskStatus.Success || status == TaskStatus.Failure)
                    {
                        child.OnEnd();
                        currentChildIndex++;
                    }

                    return status;
                }
            }

            return TaskStatus.Failure;
        }
    }
}