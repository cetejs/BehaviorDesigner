using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Tasks
{
    [TaskDescription("Similar to the sequence task, the random sequence task will return success as soon as every child task returns success.  " +
                     "The difference is that the random sequence class will run its children in a random order. The sequence task is deterministic " +
                     "in that it will always run the tasks from left to right within the tree. The random sequence task shuffles the child tasks up and then begins " +
                     "execution in a random order. Other than that the random sequence class is the same as the sequence class. It will stop running tasks " +
                     "as soon as a single task ends in failure. On a task failure it will stop executing all of the child tasks and return failure. " +
                     "If no child returns failure then it will return success.")]
    public class RandomSequence : Composite
    {
        [SerializeField]
        private int seed;
        [SerializeField]
        private bool isUseSeed;

        private List<int> childIndexList;
        private List<int> executedIndexList;
        private System.Random random;

        public override void OnAwake()
        {
            childIndexList = new List<int>(children.Count);
            executedIndexList = new List<int>(children.Count);
        }

        public override void OnStart()
        {
            base.OnStart();
            if (isUseSeed)
            {
                random = new System.Random(seed);
            }
            else
            {
                random = new System.Random();
            }

            childIndexList.Clear();
            executedIndexList.Clear();
            for (int i = 0; i < children.Count; i++)
            {
                childIndexList.Add(i);
            }

            int length = childIndexList.Count, j, temp;
            for (int i = 0; i < length; i++)
            {
                j = random.Next(length);
                temp = childIndexList[i];
                childIndexList[i] = childIndexList[j];
                childIndexList[j] = temp;
            }

            currentChildIndex = RandomNext();
        }

        public override TaskStatus OnUpdate()
        {
            if (UpdateAbort())
            {
                if (CanExecute)
                {
                    children[currentChildIndex].OnAbort();
                }

                Restart();
            }

            while (CanExecute)
            {
                if (children[currentChildIndex].IsDisabled)
                {
                    currentChildIndex = RandomNext();
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
                        task.OnEnd();
                    }

                    if (status == TaskStatus.Success)
                    {
                        currentChildIndex = RandomNext();
                    }
                    else
                    {
                        return status;
                    }
                }
            }

            return TaskStatus.Success;
        }

        public override bool UpdateAbort()
        {
            for (int i = 0; i < executedIndexList.Count; i++)
            {
                Task child = children[executedIndexList[i]];
                if (UpdateAbort(child))
                {
                    return true;
                }
            }

            return false;
        }
        
        public override void OnReset()
        {
            seed = 0;
            isUseSeed = false;
        }

        private int RandomNext()
        {
            int length = childIndexList.Count;
            if (length == 0)
            {
                return -1;
            }

            int index = childIndexList[length - 1];
            childIndexList.RemoveAt(length - 1);
            executedIndexList.Add(index);
            return index;
        }
    }
}