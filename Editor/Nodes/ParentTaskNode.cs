using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public abstract class ParentTaskNode : TaskNode
    {
        protected ParentTask parentTask;
        protected TaskPort childPort;
        protected readonly List<TaskNode> children = new List<TaskNode>();

        public TaskPort ChildPort
        {
            get { return childPort; }
        }

        public override void Init(Task task, BehaviorWindow window)
        {
            base.Init(task, window);
            parentTask = task as ParentTask;
            AddChild(parentTask.MaxChildren > 1 ? Port.Capacity.Multi : Port.Capacity.Single);
            AddDoubleClickSelection();
        }

        public override void Replace(TaskNode newNode)
        {
            base.Replace(newNode);
            if (newNode is not ParentTaskNode newParentNode || !childPort.connected)
            {
                return;
            }

            ParentTask newParentTask = newParentNode.task as ParentTask;
            for (int i = 0; i < children.Count && i < newParentTask.MaxChildren; i++)
            {
                Edge edge = BehaviorUtils.ConnectPorts(newParentNode.childPort, children[i].ParentPort);
                window.View.AddElement(edge);
            }
        }

        public override void Restore()
        {
            base.Restore();
            foreach (Task child in parentTask.Children)
            {
                TaskNode node = window.CreateNode(child);
                Edge edge = BehaviorUtils.ConnectPorts(childPort, node.ParentPort);
                node.SetPosition(child.graphPosition);
                window.View.AddElement(node);
                window.View.AddElement(edge);
            }
        }

        public override void Save()
        {
            base.Save();
            parentTask.Children.Clear();
            foreach (TaskNode child in children)
            {
                parentTask.Children.Add(child.Task);
            }

            foreach (TaskNode child in children)
            {
                child.Save();
            }
        }

        public override void Deep(Action<TaskNode> action)
        {
            base.Deep(action);
            foreach (TaskNode child in children)
            {
                child.Deep(action);
            }
        }

        public void UpdateChildIndex(TaskNode node)
        {
            if (children.Count <= 1)
            {
                return;
            }

            int index = children.IndexOf(node);
            if (index != -1)
            {
                children.RemoveAt(index);
            }

            int i = 0;
            for (; i < children.Count; i++)
            {
                if (node.GetPosition().x < children[i].GetPosition().x)
                {
                    break;
                }
            }

            children.Insert(i, node);
            index = parentTask.Children.IndexOf(node.Task);
            if (index != i)
            {
                if (index != -1)
                {
                    parentTask.Children.RemoveAt(index);
                }

                parentTask.Children.Insert(i, node.Task);
            }
        }

        public IEnumerable<TaskNode> GetChildren()
        {
            return children;
        }

        protected void AddDoubleClickSelection()
        {
            this.AddManipulator(new DoubleClickSelector(() =>
            {
                Deep(node =>
                {
                    window.View.AddToSelection(node);
                });
            }));
        }

        protected void AddChild(Port.Capacity capacity = Port.Capacity.Single)
        {
            childPort = TaskPort.Create<Edge>(Direction.Output, capacity, typeof(Port));
            childPort.portName = "";
            outputContainer.Add(childPort);

            childPort.onConnected = (edge, isManual) =>
            {
                if (children.Count <= 1)
                {
                    children.Add(edge.input.node as TaskNode);
                }

                UpdateChildIndex(edge.input.node as TaskNode);

                if (!isManual)
                {
                    window.RegisterUndo("Connect Node");
                    window.Save();
                }
            };

            childPort.onDisconnected = (edge, isManual) =>
            {
                children.Remove(edge.input.node as TaskNode);
            };
        }
    }
}