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

                    status = child.OnUpdate(false);
                    if (status != TaskStatus.Running)
                    {
                        child.OnEnd();
                    }
                }
            }

            return OnDecorate(status);
        }

        public bool UpdateAbort(bool canUpdateAbort)
        {
            for (int i = 0; i < children.Count; i++)
            {
                Task child = children[i];
                if (child.IsDisabled)
                {
                    continue;
                }

                if (child is Composite composite)
                {
                    if (composite.UpdateAbort(canUpdateAbort))
                    {
                        return true;
                    }

                    continue;
                }

                if (child is Decorator decorator)
                {
                    if (decorator.UpdateAbort(canUpdateAbort))
                    {
                        return true;
                    }

                    continue;
                }

                if (child is Conditional conditional)
                {
                    TaskStatus status = conditional.CurrentStatus;
                    if (status != conditional.OnUpdate(true))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}