using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner
{
    public abstract class ParentTask : Task
    {
        protected int lastChildIndex = -1;
        protected int currentChildIndex = 0;
        protected List<Task> children = new List<Task>();

        public List<Task> Children
        {
            get { return children; }
        }

        public virtual int MaxChildren
        {
            get { return int.MaxValue; }
        }

        public virtual bool CanRunParallelChildren
        {
            get { return false; }
        }

        public virtual bool CanExecute
        {
            get { return currentChildIndex >= 0 && currentChildIndex < MaxChildren && currentChildIndex < children.Count; }
        }

        public virtual bool CanChildStart
        {
            get
            {
                if (lastChildIndex != currentChildIndex)
                {
                    lastChildIndex = currentChildIndex;
                    return true;
                }

                return false;
            }
        }
        
        public sealed override void Bind(IBehavior behavior)
        {
            base.Bind(behavior);
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i] == null)
                {
                    children.RemoveAt(i);
                }
                else
                {
                    children[i].Bind(behavior);
                }
            }
        }

        public sealed override void Init(Behavior behavior)
        {
            base.Init(behavior);
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i] == null)
                {
                    children.RemoveAt(i);
                }
                else
                {
                    children[i].Init(behavior);
                }
            }
        }

        public sealed override void Restart()
        {
            base.Restart();
            foreach (Task child in children)
            {
                if (!child.IsDisabled)
                {
                    child.Restart();
                }
            }
        }

        public override void OnStart()
        {
            lastChildIndex = -1;
            currentChildIndex = 0;
        }
        
        public override void OnLateUpdate()
        {
            if (CanRunParallelChildren)
            {
                foreach (Task child in children)
                {
                    if (!child.IsDisabled)
                    {
                        child.OnLateUpdate();
                    }
                }
            }
            else
            {
                if (CanExecute)
                {
                    Task child = children[currentChildIndex];
                    if (!child.IsDisabled)
                    {
                        child.OnLateUpdate();
                    }
                }
            }
        }

        public override void OnFixedUpdate()
        {
            if (CanRunParallelChildren)
            {
                foreach (Task child in children)
                {
                    if (!child.IsDisabled)
                    {
                        child.OnFixedUpdate();
                    }
                }
            }
            else
            {
                if (CanExecute)
                {
                    Task child = children[currentChildIndex];
                    if (!child.IsDisabled)
                    {
                        child.OnFixedUpdate();
                    }
                }
            }
        }

        public override void OnDrawGizmos()
        {
            foreach (Task child in children)
            {
                if (!child.IsDisabled)
                {
                    child.OnDrawGizmos();
                }
            }
        }

        public override void OnCollisionEnter(Collision collision)
        {
            foreach (Task child in children)
            {
                if (!child.IsDisabled)
                {
                    child.OnCollisionEnter(collision);
                }
            }
        }

        public override void OnCollisionStay(Collision collision)
        {
            foreach (Task child in children)
            {
                if (!child.IsDisabled)
                {
                    child.OnCollisionStay(collision);
                }
            }
        }

        public override void OnCollisionExit(Collision collision)
        {
            foreach (Task child in children)
            {
                if (!child.IsDisabled)
                {
                    child.OnCollisionExit(collision);
                }
            }
        }

        public override void OnTriggerEnter(Collider other)
        {
            foreach (Task child in children)
            {
                if (!child.IsDisabled)
                {
                    child.OnTriggerEnter(other);
                }
            }
        }

        public override void OnTriggerStay(Collider other)
        {
            foreach (Task child in children)
            {
                if (!child.IsDisabled)
                {
                    child.OnTriggerStay(other);
                }
            }
        }

        public override void OnTriggerExit(Collider other)
        {
            foreach (Task child in children)
            {
                if (!child.IsDisabled)
                {
                    child.OnTriggerExit(other);
                }
            }
        }

        public override void OnTriggerEnter2D(Collider2D other)
        {
            foreach (Task child in children)
            {
                if (!child.IsDisabled)
                {
                    child.OnTriggerEnter2D(other);
                }
            }
        }

        public override void OnTriggerStay2D(Collider2D other)
        {
            foreach (Task child in children)
            {
                if (!child.IsDisabled)
                {
                    child.OnTriggerStay2D(other);
                }
            }
        }

        public override void OnTriggerExit2D(Collider2D other)
        {
            foreach (Task child in children)
            {
                if (!child.IsDisabled)
                {
                    child.OnTriggerExit2D(other);
                }
            }
        }

        public override void OnControllerColliderHit(ControllerColliderHit hit)
        {
            foreach (Task child in children)
            {
                if (!child.IsDisabled)
                {
                    child.OnControllerColliderHit(hit);
                }
            }
        }

        public override void OnAnimatorIK(int layerIndex)
        {
            foreach (Task child in children)
            {
                if (!child.IsDisabled)
                {
                    child.OnAnimatorIK(layerIndex);
                }
            }
        }

        public override void OnAbort()
        {
            if (CanRunParallelChildren)
            {
                foreach (Task child in children)
                {
                    if (!child.IsDisabled)
                    {
                        child.OnAbort();
                    }
                }
            }
            else
            {
                if (CanExecute)
                {
                    Task child = children[currentChildIndex];
                    if (!child.IsDisabled)
                    {
                        child.OnAbort();
                    }
                }
            }
        }

        public virtual bool UpdateAbort()
        {
            return false;
        }
    }
}