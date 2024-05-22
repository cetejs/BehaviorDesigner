using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner
{
    public abstract class ParentTaskNode : TaskNode
    {
        protected ParentTask parentTask;
        protected Port output;
        private VisualElement collapseBtn;
        private VisualElement connectionCollapsed;

        public bool IsCollapsed
        {
            get { return parentTask.IsCollapsed; }
            set
            {
                if (parentTask.IsCollapsed != value)
                {
                    window.UndoRecord("BehaviorTree Collapse Task");
                    parentTask.IsCollapsed = value;
                    UpdateCollapsedStatus();
                }
            }
        }

        public ParentTask ParentTask
        {
            get { return parentTask; }
        }

        public Port Output
        {
            get { return output; }
        }

        public ParentTaskNode()
        {
            AddToClassList("parentNode");
            collapseBtn = this.Q("collapse-btn");
            collapseBtn.AddToClassList("collapse-btn");
            collapseBtn.AddManipulator(new Clickable(() => IsCollapsed = !IsCollapsed));
            AddDoubleClickSelection();
        }

        public override void Init(Task task, BehaviorWindow window)
        {
            parentTask = task as ParentTask;
            base.Init(task, window);
        }

        protected override void OnVisible()
        {
            base.OnVisible();
            output.visible = visible && !IsCollapsed;
            connectionCollapsed.visible = visible && IsCollapsed;
        }

        public override void Restore()
        {
            base.Restore();
            UpdateCollapsedStatus();
        }

        public override bool DeepCall(Func<TaskNode, bool> action)
        {
            if (!base.DeepCall(action))
            {
                return false;
            }

            foreach (Task child in parentTask.Children)
            {
                TaskNode childNode = window.View.FindNode<TaskNode>(child);
                childNode.DeepCall(action);
            }

            return true;
        }

        public override void OnSelected()
        {
            if (IsCollapsed)
            {
                DeepCall(taskNode =>
                {
                    if (taskNode == this)
                    {
                        return true;
                    }

                    window.View.AddToSelection(taskNode);
                    return true;
                });
            }

            base.OnSelected();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            string setCollapseAction = parentTask.IsCollapsed ? "Expand" : "Collapse";
            evt.menu.AppendAction(setCollapseAction, action =>
            {
                IsCollapsed = !IsCollapsed;
            }, callback => DropdownMenuAction.Status.Normal);
        }

        protected void CreateOutputPort()
        {
            Port.Capacity capacity = parentTask.MaxChildren > 1 ? Port.Capacity.Multi : Port.Capacity.Single;
            output = InstantiatePort(Orientation.Vertical, Direction.Output, capacity, typeof(Port));
            output.portName = "";
            outputContainer.Add(output);

            connectionCollapsed = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    alignContent = Align.Center,
                    width = 26,
                    height = 6,
                    backgroundImage = EditorBehaviorUtility.LoadTexture("Icons/TaskConnectionCollapsed")
                }
            };
            outputContainer.Add(connectionCollapsed);
        }

        protected void UpdateCollapsedStatus()
        {
            if (IsCollapsed)
            {
                output.visible = false;
                connectionCollapsed.visible = visible;
                DeepCall(taskNode =>
                {
                    if (taskNode == this)
                    {
                        return true;
                    }

                    taskNode.IsVisible = false;
                    foreach (Edge edge in taskNode.Input.connections)
                    {
                        edge.visible = false;
                    }

                    if (selected)
                    {
                        window.View.AddToSelection(taskNode);
                    }

                    if (taskNode is ParentTaskNode parentNode && parentNode.IsCollapsed)
                    {
                        return false;
                    }

                    return true;
                });
            }
            else
            {
                output.visible = visible;
                connectionCollapsed.visible = false;
                DeepCall(taskNode =>
                {
                    if (taskNode == this)
                    {
                        return true;
                    }

                    if (taskNode.selected)
                    {
                        window.View.RemoveFromSelection(taskNode);
                    }

                    taskNode.IsVisible = visible;
                    foreach (Edge edge in taskNode.Input.connections)
                    {
                        edge.visible = visible;
                    }

                    if (taskNode is ParentTaskNode parentNode && parentNode.IsCollapsed)
                    {
                        return false;
                    }

                    return true;
                });
            }

            string icon = parentTask.IsCollapsed ? "Icons/TaskExpandIcon" : "Icons/TaskCollapseIcon";
            collapseBtn.style.backgroundImage = EditorBehaviorUtility.LoadTexture(icon);
        }

        protected void AddDoubleClickSelection()
        {
            this.AddManipulator(new DoubleClickSelector(() =>
            {
                DeepCall(node =>
                {
                    window.View.AddToSelection(node);
                    return true;
                });
            }));
        }
    }
}