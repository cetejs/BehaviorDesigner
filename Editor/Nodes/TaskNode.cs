using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public abstract class TaskNode : Node
    {
        protected Task task;
        protected TaskPort parentPort;
        protected ParentTaskNode parentNode;
        protected BehaviorWindow window;
        private TextField nameInput;
        private TextField commentInput;
        private Label commentLabel;
        private Rect position;
        private VisualElement breakpoint;
        private VisualElement statusIcon;
        private VisualElement nodeBorder;

        private static readonly float ColorLerpSpeed = 2f;
        private static readonly Color DefaultColor = new Color(0.2f, 0.2f, 0.2f);
        private static readonly Color DisableColor = new Color(0.5f, 0.5f, 0.5f);
        private static readonly Color RunningColor = new Color(0.2f, 0.55f, 0.25f);

        protected virtual bool IsAddComment { get; }

        private readonly List<PriorityResolver> priorityResolvers = new List<PriorityResolver>();

        private static MethodInfo disconnectAll;
        private static MethodInfo disconnectAllStatus;

        public TaskNode() : base(AssetDatabase.GetAssetPath(BehaviorUtils.Load<VisualTreeAsset>("UXML/TaskNode")))
        {
            nameInput = new TextField("Name");
            nameInput.SetEnabled(false);
            commentInput = new TextField("Comment");
            commentInput.name = "comment-input";
            commentInput.multiline = true;
            commentLabel = this.Q<Label>("comment-label");
            breakpoint = this.Q("breakpoint");
            statusIcon = this.Q("status");
            nodeBorder = this.Q("node-border");
            styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/TaskNode"));

            if (disconnectAll == null)
            {
                disconnectAll = typeof(Node).GetMethod("DisconnectAll", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            if (disconnectAllStatus == null)
            {
                disconnectAllStatus = typeof(Node).GetMethod("DisconnectAllStatus", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            if (IsAddComment)
            {
                AddComment();
            }
        }

        public Task Task
        {
            get { return task; }
        }

        public TaskPort ParentPort
        {
            get { return parentPort; }
        }

        public ParentTaskNode ParentNode
        {
            get { return parentNode; }
        }

        public virtual void Init(Task task, BehaviorWindow window)
        {
            this.task = task;
            this.window = window;
            window.onUpdate += Update;
            AddParent();
        }

        public virtual void Replace(Task task)
        {
            this.task = task;
            Restore();
        }

        public virtual void Restore()
        {
            priorityResolvers.Clear();
            Type type = task.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                if (field.IsInitOnly)
                {
                    continue;
                }

                if (!field.IsPublic && field.GetCustomAttribute<SerializeField>() == null)
                {
                    continue;
                }

                if (field.GetCustomAttribute<HideInInspector>() != null)
                {
                    continue;
                }

                IFieldResolver resolver = window.CreateField(field);
                resolver.Register();
                resolver.Restore(task);

                FieldPriorityAttribute priorityAttribute = field.GetCustomAttribute<FieldPriorityAttribute>();
                int priority = priorityAttribute == null ? int.MaxValue : priorityAttribute.priority;
                int index = 0;
                for (int i = priorityResolvers.Count - 1; i >= 0; i--)
                {
                    if (priority >= priorityResolvers[i].priority)
                    {
                        index = i + 1;
                        break;
                    }
                }

                priorityResolvers.Insert(index, new PriorityResolver()
                {
                    priority = priority,
                    resolver = resolver
                });
            }

            TaskNameAttribute attribute = type.GetCustomAttribute<TaskNameAttribute>();
            if (attribute != null)
            {
                title = ObjectNames.NicifyVariableName(attribute.name);
            }
            else
            {
                title = ObjectNames.NicifyVariableName(type.Name);
            }

            UpdateDisableStatus();
            OnTaskUpdate(task.CurrentStatus, true);
            SetComment(task.comment);
            commentInput.value = task.comment;
            breakpoint.visible = task.breakpoint;
            task.UpdateNotifyOnEditor = OnTaskUpdate;
        }

        public virtual void Save()
        {
            task.comment = commentInput.value;
            task.graphPosition = position;
            foreach (PriorityResolver priorityResolver in priorityResolvers)
            {
                priorityResolver.resolver.Save(task);
            }
        }

        public virtual void Deep(Action<TaskNode> action)
        {
            action?.Invoke(this);
        }

        public virtual void Reset()
        {
            window.RegisterUndo("Reset Task");
            task.OnReset();
            Restore();
            window.Save();
        }

        public void OnGUI(VisualElement container)
        {
            container.Clear();
            nameInput.value = title;
            container.Add(nameInput);
            if (IsAddComment)
            {
                container.Add(commentInput);
            }

            foreach (PriorityResolver priorityResolver in priorityResolvers)
            {
                container.Add(priorityResolver.resolver.EditorField);
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            position = newPos;
        }

        public override void UpdatePresenterPosition()
        {
            window.RegisterUndo("Update NodePosition");
            task.graphPosition = GetPosition();
            parentNode?.UpdateChildIndex(this);
            window.Save();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            AddScriptMenuItem(evt);

            evt.menu.AppendAction("Replace", action =>
            {
                Vector2 pos = window.position.position + action.eventInfo.mousePosition;
                ReplaceMenuWindowProvider provider = ScriptableObject.CreateInstance<ReplaceMenuWindowProvider>();
                provider.Init(window, this);
                SearchWindow.Open(new SearchWindowContext(pos), provider);
            }, callback => DropdownMenuAction.Status.Normal);

            string setBreakpointAction = !task.breakpoint ? "Set Breakpoint" : "Remove Breakpoint";
            evt.menu.AppendAction(setBreakpointAction, action =>
            {
                window.RegisterUndo($"{setBreakpointAction} TaskNode");
                task.breakpoint = !task.breakpoint;
                breakpoint.visible = task.breakpoint;
                window.Save();
            }, callback => DropdownMenuAction.Status.Normal);

            string setEnableAction = task.IsDisabled ? "Set Enable" : "Set Disable";
            evt.menu.AppendAction(setEnableAction, action =>
            {
                window.RegisterUndo($"{setEnableAction} TaskNode");
                task.IsDisabled = !task.IsDisabled;
                UpdateDisableStatus();
                window.Save();
            }, callback => DropdownMenuAction.Status.Normal);

            evt.menu.AppendAction("Disconnect all", action =>
            {
                window.RegisterUndo("DisconnectAll TaskNode");
                disconnectAll.Invoke(this, new object[] {action});
                window.Save();
            }, callback =>
            {
                return (DropdownMenuAction.Status) disconnectAllStatus.Invoke(this, new object[] {callback});
            });
            evt.menu.AppendSeparator();
        }

        protected void OnTaskUpdate(TaskStatus status, bool isUpdateAbort)
        {
            if (selected)
            {
                foreach (PriorityResolver priorityResolver in priorityResolvers)
                {
                    priorityResolver.resolver.Restore(task);
                }
            }

            if (!isUpdateAbort && status != TaskStatus.Inactive)
            {
                nodeBorder.style.backgroundColor = RunningColor;
            }

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

            if (!string.IsNullOrEmpty(iconName))
            {
                statusIcon.visible = true;
                statusIcon.style.backgroundImage = BehaviorUtils.Load<Texture2D>($"Icons/{iconName}");
            }
            else
            {
                statusIcon.visible = false;
            }
        }

        protected void AddScriptMenuItem(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Edit Script", action =>
            {
                BehaviorUtils.OpenScript(task);
            }, callback => DropdownMenuAction.Status.Normal);
            evt.menu.AppendAction("Locate Script", action =>
            {
                BehaviorUtils.SelectScript(task);
            }, callback => DropdownMenuAction.Status.Normal);
            evt.menu.AppendSeparator();
        }

        protected void Update()
        {
            if (task.IsDisabled)
            {
                return;
            }

            Color color = nodeBorder.style.backgroundColor.value;

            nodeBorder.style.backgroundColor = Color.LerpUnclamped(color, DefaultColor, Time.deltaTime * ColorLerpSpeed);
        }

        private void UpdateDisableStatus()
        {
            if (task.IsDisabled)
            {
                nodeBorder.style.backgroundColor = DisableColor;
            }
            else
            {
                nodeBorder.style.backgroundColor = DefaultColor;
            }
        }

        private void AddParent()
        {
            parentPort = TaskPort.Create<Edge>(Direction.Input, Port.Capacity.Single, typeof(Port));
            parentPort.portName = "";
            inputContainer.Add(parentPort);

            parentPort.onConnected = (edge, isManual) =>
            {
                parentNode = edge.output.node as ParentTaskNode;
            };

            parentPort.onDisconnected = (port, isManual) =>
            {
                parentNode = null;
            };
        }

        private void AddComment()
        {
            commentInput.RegisterValueChangedCallback(evt =>
            {
                window.RegisterUndo("Update Comment");
                SetComment(commentInput.value);
                task.comment = commentInput.value;
                window.Save();
            });
        }

        private void SetComment(string text)
        {
            commentLabel.text = text;
            if (string.IsNullOrEmpty(text))
            {
                commentLabel.parent.style.display = DisplayStyle.None;
            }
            else
            {
                commentLabel.parent.style.display = DisplayStyle.Flex;
            }
        }

        private struct PriorityResolver
        {
            public int priority;
            public IFieldResolver resolver;
        }
    }
}