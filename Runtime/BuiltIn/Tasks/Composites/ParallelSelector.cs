using System.Collections.Generic;

namespace BehaviorDesigner.Tasks
{
    [TaskDescription("Similar to the selector task, the parallel selector task will return success as soon as a child task returns success. " +
                     "The difference is that the parallel task will run all of its children tasks simultaneously versus running each task one at a time. " +
                     "If one tasks returns success the parallel selector task will end all of the child tasks and return success. " +
                     "If every child task returns failure then the parallel selector task will return failure.")]
    public class ParallelSelector : Composite
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
            if (UpdateAbort())
            {
                if (CanExecute)
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
                            child.OnAbort();
                        }
                    }
                }

                RestartAbort();
            }

            TaskStatus status = TaskStatus.Failure;

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

                    TaskStatus childStatus = child.Update();

                    if (childStatus == TaskStatus.Failure)
                    {
                        child.OnEnd();
                        taskIsRunning[i] = false;
                    }
                    else if (status != TaskStatus.Success)
                    {
                        status = childStatus;
                    }
                }
            }

            if (status == TaskStatus.Success)
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

        protected override bool UpdateAbort(Task task)
        {
            if (task is Composite && task.CurrentStatus != TaskStatus.Running)
            {
                return false;
            }

            return base.UpdateAbort(task);
        }
    }
}