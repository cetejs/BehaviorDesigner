namespace BehaviorDesigner
{
    public class Decorator : ParentTask
    {
        public override int MaxChildren
        {
            get { return 1; }
        }

        public virtual TaskStatus OnDecorate(TaskStatus status)
        {
            return status;
        }

        public override TaskStatus OnUpdate()
        {
            TaskStatus status = TaskStatus.Failure;
            if (CanExecute)
            {
                Task child = children[currentChildIndex];
                if (!child.IsDisabled)
                {
                    if (CanChildStart)
                    {
                        child.OnStart();
                    }

                    status = child.Update();
                    if (status != TaskStatus.Running)
                    {
                        child.OnEnd();
                    }
                }
            }

            return OnDecorate(status);
        }

        public override bool UpdateAbort()
        {
            for (int i = 0; i < children.Count; i++)
            {
                Task child = children[i];
                if (child.IsDisabled)
                {
                    continue;
                }

                if (child is ParentTask parentTask)
                {
                    if (child is Composite composite && composite.AbortType <= AbortType.Self)
                    {
                        continue;
                    }

                    if (parentTask.UpdateAbort())
                    {
                        return true;
                    }

                    continue;
                }

                if (child is Conditional conditional)
                {
                    TaskStatus status = conditional.CurrentStatus;
                    if (status != conditional.Update())
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}