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
        private readonly List<Task> runningTasks = new List<Task>();

        public override bool CanRunParallelChildren
        {
            get { return true; }
        }

        public override void OnStart()
        {
            base.OnStart();
            isChildrenRunning = false;
            runningTasks.Clear();
            runningTasks.AddRange(children);
        }

        public override TaskStatus OnUpdate()
        {
            if (UpdateAbort())
            {
                if (CanExecute)
                {
                    for (int i = 0; i < runningTasks.Count; i++)
                    {
                        Task child = runningTasks[i];
                        if (!child.IsDisabled)
                        {
                            child.OnAbort();
                        }
                    }
                }

                Restart();
            }

            TaskStatus status = TaskStatus.Failure;

            for (int i = 0; i < runningTasks.Count; i++)
            {
                Task child = runningTasks[i];
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
                        runningTasks.RemoveAt(i--);
                    }
                    else if (status != TaskStatus.Success)
                    {
                        status = childStatus;
                    }
                }
            }

            if (status == TaskStatus.Success)
            {
                for (int i = 0; i < runningTasks.Count; i++)
                {
                    Task child = runningTasks[i];
                    if (!child.IsDisabled)
                    {
                        child.OnEnd();
                    }
                }
            }

            isChildrenRunning = true;
            return status;
        }
    }
}