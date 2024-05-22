using System;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    public class Composite : ParentTask
    {
        [SerializeField]
        protected AbortType abortType;

        public AbortType AbortType
        {
            get { return abortType; }
        }

        public virtual void RestartAbort()
        {
            currentChildIndex = abortChildIndex;
            if (children[currentChildIndex] is Composite composite)
            {
                lastChildIndex = currentChildIndex;
                composite.RestartAbort();
            }
            else
            {
                lastChildIndex = currentChildIndex - 1;
            }
        }

        public virtual bool UpdateAbort(bool canUpdateAbort)
        {
            int count = CanRunParallelChildren ? children.Count : currentChildIndex;
            for (int i = 0; i < count; i++)
            {
                Task child = children[i];
                if (child.IsDisabled)
                {
                    continue;
                }

                if (UpdateAbort(child, canUpdateAbort))
                {
                    abortChildIndex = i;
                    return true;
                }
            }

            return false;
        }

        protected virtual bool UpdateAbort(Task task, bool canUpdateAbort)
        {
            canUpdateAbort = (abortType == AbortType.Both || abortType == AbortType.Self) && CurrentStatus == TaskStatus.Running ||
                             (abortType == AbortType.Both || abortType == AbortType.LowerPriority) && CurrentStatus != TaskStatus.Running && canUpdateAbort;

            if (task is Composite composite)
            {
                return composite.UpdateAbort(CurrentStatus == TaskStatus.Running || canUpdateAbort);
            }

            if (task is Decorator decorator)
            {
                return decorator.UpdateAbort(CurrentStatus == TaskStatus.Running || canUpdateAbort);
            }

            if (canUpdateAbort)
            {
                if (task is Conditional conditional)
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

        public override void OnReset()
        {
            abortType = AbortType.None;
        }
    }
}