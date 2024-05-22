using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner
{
    public abstract class TaskNode : Node
    {
        protected Task task;
        protected BehaviorWindow window;
        protected Port input;
        protected string taskIconName;
        private VisualElement taskIcon;
        private VisualElement statusIcon;
        private VisualElement nodeBorder;
        private VisualElement breakpoint;
        private VisualElement disableBtn;
        private string statusIconName;

        protected static readonly float ColorLerpSpeed = 0.25f;
        protected static readonly Color DefaultColor = new Color(0.2f, 0.2f, 0.2f);
        protected static readonly Color DisableColor = new Color(0.5f, 0.5f, 0.5f);
        protected static readonly Color RunningColor = new Color(0.2f, 0.55f, 0.25f);

        public Task Task
        {
            get { return task; }
        }

        public Port Input
        {
            get { return input; }
        }

        public bool IsVisible
        {
            get { return visible; }
            set
            {
                if (visible != value)
                {
                    visible = value;
                    OnVisible();
                }
            }
        }

        public bool IsDisabled
        {
            get { return task.IsDisabled; }
            set
            {
                if (task.IsDisabled != value)
                {
                    window.UndoRecord("BehaviorTree Disable Task");
                    task.IsDisabled = value;
                    UpdateDisableStatus();
                }
            }
        }

        public TaskNode() : base(AssetDatabase.GetAssetPath(EditorBehaviorUtility.Load<VisualTreeAsset>("UXML/TaskNode")))
        {
            statusIcon = this.Q("status");
            taskIcon = this.Q("icon");
            nodeBorder = this.Q("node-border");
            breakpoint = this.Q("breakpoint");
            disableBtn = this.Q("disable-btn");
            disableBtn.AddManipulator(new Clickable(() => IsDisabled = !IsDisabled));
            breakpoint.style.backgroundImage = EditorBehaviorUtility.LoadTexture("Icons/BreakpointIcon");
            styleSheets.Add(EditorBehaviorUtility.Load<StyleSheet>("Styles/TaskNode"));
        }

        public virtual void Init(Task task, BehaviorWindow window)
        {
            this.task = task;
            this.window = window;
            viewDataKey = task.Guid;
            window.onUpdate += OnUpdate;
            task.UpdateNotifyWithAbort = OnUpdateNotifyWithAbort;
            CreatePorts();
        }

        protected virtual void OnUpdateNotifyWithAbort(bool isUpdateAbort)
        {
            if (task.IsBreakpoint)
            {
                EditorApplication.isPaused = true;
            }

            if (!visible)
            {
                return;
            }

            if (!isUpdateAbort && task.CurrentStatus != TaskStatus.Inactive)
            {
                nodeBorder.style.backgroundColor = RunningColor;
            }

            UpdateStatus(task.CurrentStatus, isUpdateAbort);
        }

        protected virtual void OnUpdate()
        {
            if (!visible)
            {
                return;
            }

            if (selected)
            {
                title = task.Name;
            }

            if (task.IsDisabled)
            {
                return;
            }

            if (task.CurrentStatus == TaskStatus.Running)
            {
                nodeBorder.style.backgroundColor = RunningColor;
            }
            else
            {
                Color color = nodeBorder.style.backgroundColor.value;
                nodeBorder.style.backgroundColor = Color.Lerp(color, DefaultColor, ColorLerpSpeed);
            }
        }

        protected virtual void OnVisible()
        {
            statusIcon.visible = visible && !string.IsNullOrEmpty(statusIconName);
            breakpoint.visible = visible && task.IsBreakpoint;
        }

        protected virtual void CreatePorts()
        {
        }

        protected virtual void UpdateTaskIcon()
        {
            TaskIconAttribute attribute = task.GetType().GetCustomAttribute<TaskIconAttribute>();
            if (attribute != null)
            {
                taskIconName = attribute.iconName;
            }

            taskIcon.style.backgroundImage = EditorBehaviorUtility.LoadTexture(taskIconName);
        }

        protected void CreateInputPort()
        {
            input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(Port));
            input.portName = "";
            inputContainer.Add(input);
        }

        protected void UpdateStatus(TaskStatus status, bool isUpdateAbort)
        {
            string iconName = null;
            switch (status)
            {
                case TaskStatus.Success:
                    iconName = isUpdateAbort ? "ExecutionSuccessRepeat" : "ExecutionSuccess";
                    break;
                case TaskStatus.Failure:
                    iconName = isUpdateAbort ? "ExecutionFailureRepeat" : "ExecutionFailure";
                    break;
            }

            if (iconName != statusIconName)
            {
                statusIconName = iconName;
                if (!string.IsNullOrEmpty(iconName))
                {
                    statusIcon.visible = visible;
                    statusIcon.style.backgroundImage = EditorBehaviorUtility.LoadTexture($"Icons/{iconName}");
                }
                else
                {
                    statusIcon.visible = false;
                }
            }
        }

        protected void UpdateDisableStatus()
        {
            nodeBorder.style.backgroundColor = task.IsDisabled ? DisableColor : DefaultColor;
            string icon = task.IsDisabled ? "Icons/TaskEnableIcon" : "Icons/TaskDisableIcon";
            disableBtn.style.backgroundImage = EditorBehaviorUtility.LoadTexture(icon);
        }

        protected void UpdateBreakpointStatus()
        {
            breakpoint.visible = visible && task.IsBreakpoint;
        }

        public virtual void Restore()
        {
            title = task.Name;
            UpdateTaskIcon();
            SetPosition(task.graphPosition);
            UpdateStatus(task.CurrentStatus, false);
            UpdateDisableStatus();
            UpdateBreakpointStatus();
        }

        public virtual bool DeepCall(Func<TaskNode, bool> action)
        {
            return action.Invoke(this);
        }

        public override void OnSelected()
        {
            if (!visible)
            {
                return;
            }

            base.OnSelected();
            window.View.onNodeSelected?.Invoke(this);
        }

        public override void OnUnselected()
        {
            if (!visible)
            {
                return;
            }

            base.OnUnselected();
            window.View.onNodeUnselected?.Invoke(this);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Edit Script", action =>
            {
                EditorBehaviorUtility.OpenScript(task);
            }, callback => DropdownMenuAction.Status.Normal);
            evt.menu.AppendAction("Locate Script", action =>
            {
                EditorBehaviorUtility.SelectScript(task);
            }, callback => DropdownMenuAction.Status.Normal);
            evt.menu.AppendSeparator();
            string setEnableAction = task.IsDisabled ? "Set Enable" : "Set Disable";
            evt.menu.AppendAction(setEnableAction, action =>
            {
                IsDisabled = !IsDisabled;
            }, callback => DropdownMenuAction.Status.Normal);
            string setBreakpointAction = task.IsBreakpoint ? "Remove Breakpoint" : "Set Breakpoint";
            evt.menu.AppendAction(setBreakpointAction, action =>
            {
                window.UndoRecord("BehaviorTree Breakpoint Task");
                task.IsBreakpoint = !task.IsBreakpoint;
                UpdateBreakpointStatus();
            }, callback => DropdownMenuAction.Status.Normal);
        }
    }
}