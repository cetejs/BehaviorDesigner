using System.Collections.Generic;

namespace BehaviorDesigner.Tasks
{
    [TaskDescription("Similar to the sequence task, the parallel task will run each child task until a child task returns failure. " +
                     "The difference is that the parallel task will run all of its children tasks simultaneously versus running each task one at a time. " +
                     "Like the sequence class, the parallel task will return success once all of its children tasks have return success. " +
                     "If one tasks returns failure the parallel task will end all of the child tasks and return failure.")]
    public class Parallel : Composite
    {
        private bool isChildrenRunning;
        private readonly List<Task> runningTasks = new List<Task>();

        public override bool CanRunParallelChildren
        {
            get { return true; }
        }

        public override void OnStart()
        {
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

            TaskStatus status = TaskStatus.Success;

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

                    if (childStatus == TaskStatus.Success)
                    {
                        child.OnEnd();
                        runningTasks.RemoveAt(i--);
                    }
                    else if (status != TaskStatus.Failure)
                    {
                        status = childStatus;
                    }
                }
            }

            if (status == TaskStatus.Failure)
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