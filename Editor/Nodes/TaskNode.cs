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

        protected virtual bool IsAddComment { get; }

        private readonly List<IFieldResolver> resolvers = new List<IFieldResolver>();

        private static MethodInfo disconnectAll;
        private static MethodInfo disconnectAllStatus;

        public TaskNode() : base(AssetDatabase.GetAssetPath(BehaviorUtils.Load<VisualTreeAsset>("UXML/TaskNode")))
        {
            nameInput = new TextField("Name");
            nameInput.SetEnabled(false);
            commentInput = new TextField("Comment");
            commentInput.name = "comment-input";
            commentInput.multiline = true;
            commentLabel = this.Q<Label>("comment");
            breakpoint = this.Q("breakpoint");
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
            AddParent();
        }

        public virtual void Replace(Task task)
        {
            this.task = task;
            Restore();
        }

        public virtual void Restore()
        {
            resolvers.Clear();
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
                resolvers.Add(resolver);
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

            MarkAsExecuted(task.CurrentStatus);
            SetComment(task.comment);
            commentInput.value = task.comment;
            breakpoint.visible = task.breakpoint;
            task.UpdateNotifyOnEditor = OnTaskUpdate;
        }

        public virtual void Save()
        {
            task.comment = commentInput.value;
            task.graphPosition = position;
            foreach (IFieldResolver resolver in resolvers)
            {
                resolver.Save(task);
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

            foreach (IFieldResolver resolver in resolvers)
            {
                container.Add(resolver.EditorField);
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
            string setEnableAction = task.IsDisabled ? "Set Enable" : "Set Disable";
            evt.menu.AppendAction(setEnableAction, action =>
            {
                window.RegisterUndo($"{setEnableAction} TaskNode");
                task.IsDisabled = !task.IsDisabled;
                MarkAsExecuted(task.CurrentStatus);
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

        protected virtual void OnTaskUpdate(TaskStatus status)
        {
            if (selected)
            {
                foreach (IFieldResolver resolver in resolvers)
                {
                    resolver.Restore(task);
                }
            }

            MarkAsExecuted(status);
        }

        protected virtual void MarkAsExecuted(TaskStatus status)
        {
            RemoveFromClassList("disable");
            RemoveFromClassList("running");
            RemoveFromClassList("failure");
            RemoveFromClassList("success");

            if (task.IsDisabled)
            {
                AddToClassList("disable");
                return;
            }

            switch (status)
            {
                case TaskStatus.Failure:
                    AddToClassList("failure");
                    break;
                case TaskStatus.Running:
                    AddToClassList("running");
                    break;
                case TaskStatus.Success:
                    AddToClassList("success");
                    break;
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

        protected void AddReplaceMenuItem(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Replace", action =>
            {
                Vector2 pos = window.position.position + action.eventInfo.mousePosition;
                ReplaceMenuWindowProvider provider = ScriptableObject.CreateInstance<ReplaceMenuWindowProvider>();
                provider.Init(window, this);
                SearchWindow.Open(new SearchWindowContext(pos), provider);
            }, callback => DropdownMenuAction.Status.Normal);
        }

        protected void AddBreakpointMenuItem(ContextualMenuPopulateEvent evt)
        {
            string setBreakpointAction = !task.breakpoint ? "Set Breakpoint" : "Remove Breakpoint";
            evt.menu.AppendAction(setBreakpointAction, action =>
            {
                window.RegisterUndo($"{setBreakpointAction} TaskNode");
                task.breakpoint = !task.breakpoint;
                breakpoint.visible = task.breakpoint;
                window.Save();
            }, callback => DropdownMenuAction.Status.Normal);
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
                commentLabel.style.display = DisplayStyle.None;
            }
            else
            {
                commentLabel.style.display = DisplayStyle.Flex;
            }
        }
    }
}