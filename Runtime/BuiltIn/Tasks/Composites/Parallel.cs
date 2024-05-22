using System.Collections.Generic;

namespace BehaviorDesigner
{
    [TaskIcon("Icons/ParallelIcon")]
    [TaskDescription("Similar to the sequence task, the parallel task will run each child task until a child task returns failure. " +
                     "The difference is that the parallel task will run all of its children tasks simultaneously versus running each task one at a time. " +
                     "Like the sequence class, the parallel task will return success once all of its children tasks have return success. " +
                     "If one tasks returns failure the parallel task will end all of the child tasks and return failure.")]
    public class Parallel : Composite
    {
        private bool isChildrenRunning;
        private readonly List<bool> taskIsRunning = new List<bool>();

        public override bool CanRunParallelChildren
        {
            get { return true; }
        }

        public override void OnStart()
        {
            base.OnStart();
            isChildrenRunning = false;
            taskIsRunning.Clear();
            for (int i = 0; i < children.Count; i++)
            {
                taskIsRunning.Add(true);
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (UpdateAbort(true))
            {
                if (CanExecute)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        if (!taskIsRunning[i])
                        {
                            continue;
                        }

                        if (!children[i].IsDisabled)
                        {
                            children[i].OnAbort();
                        }
                    }
                }

                RestartAbort();
            }

            TaskStatus status = TaskStatus.Success;

            for (int i = 0; i < children.Count; i++)
            {
                if (!taskIsRunning[i])
                {
                    continue;
                }

                Task child = children[i];
                if (!child.IsDisabled)
                {
                    if (!isChildrenRunning)
                    {
                        child.OnStart();
                    }

                    TaskStatus childStatus = child.OnUpdate(false);

                    if (childStatus == TaskStatus.Success)
                    {
                        child.OnEnd();
                        taskIsRunning[i] = false;
                    }
                    else if (status != TaskStatus.Failure)
                    {
                        status = childStatus;
                    }
                }
            }

            if (status == TaskStatus.Failure)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    if (!taskIsRunning[i])
                    {
                        continue;
                    }

                    Task child = children[i];
                    if (!child.IsDisabled)
                    {
                        child.OnEnd();
                    }
                }
            }

            isChildrenRunning = true;
            return status;
        }

        public override void RestartAbort()
        {
            if (children[abortChildIndex] is Composite child)
            {
                taskIsRunning[abortChildIndex] = true;
                child.RestartAbort();
            }
            else
            {
                OnStart();
            }
        }

        protected override bool UpdateAbort(Task task, bool canUpdateAbort)
        {
            if (task is Composite && task.CurrentStatus != TaskStatus.Running)
            {
                return false;
            }

            return base.UpdateAbort(task, canUpdateAbort);
        }
    }
}