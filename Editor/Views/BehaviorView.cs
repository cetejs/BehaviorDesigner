using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner
{
    public class BehaviorView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviorView, UxmlTraits> {}

        private BehaviorWindow window;
        private static List<Task> copyTasks = new List<Task>();

        public Action<TaskNode> onNodeSelected;
        public Action<TaskNode> onNodeUnselected;

        public IBehavior Behavior
        {
            get { return window.Behavior; }
        }

        public BehaviorSource Source
        {
            get { return window.Source; }
        }

        public void Init(BehaviorWindow window)
        {
            this.window = window;
            AddDefaultManipulator();
            RegisterCreationRequest();
            RegisterCopyAndPaste();
        }

        public void Refresh()
        {
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;
            if (Source == null)
            {
                return;
            }

            for (int i = Source.Tasks.Count - 1; i >= 0; i--)
            {
                if (Source.Tasks[i] == null)
                {
                    Source.Tasks.RemoveAt(i);
                    Debug.LogError($"{Behavior.Object.name} has task is null to remove");
                    continue;
                }

                if (Source.Tasks[i] is ParentTask parentTask)
                {
                    for (int j = parentTask.Children.Count - 1; j >= 0; j--)
                    {
                        if (parentTask.Children[j] == null)
                        {
                            parentTask.Children.RemoveAt(j);
                            Debug.LogError($"{Behavior.Object.name} task {parentTask.Name} has child is null to remove");
                        }
                    }
                }
            }

            foreach (Task task in Source.Tasks)
            {
                AddElement(window.CreateNode(task));
            }

            foreach (Task task in Source.Tasks)
            {
                if (task is ParentTask parentTask)
                {
                    ParentTaskNode parentNode = FindNode<ParentTaskNode>(parentTask);
                    foreach (Task childTask in parentTask.Children)
                    {
                        ConnectTo(parentNode, childTask);
                    }
                }
            }

            foreach (Task task in Source.Tasks)
            {
                TaskNode taskNode = FindNode<TaskNode>(task);
                taskNode.Restore();
            }

            viewTransformChanged -= OnViewTransformChanged;
            UpdateViewTransform();
            viewTransformChanged += OnViewTransformChanged;
        }

        public T FindNode<T>(Task task) where T : TaskNode
        {
            return GetNodeByGuid(task.Guid) as T;
        }

        public void ConnectTo(ParentTask parentTask, Task childTask)
        {
            ParentTaskNode parentNode = FindNode<ParentTaskNode>(parentTask);
            TaskNode childNode = FindNode<TaskNode>(childTask);
            if (parentNode != null && childNode != null)
            {
                Edge edge = parentNode.Output.ConnectTo(childNode.Input);
                AddElement(edge);
            }
        }
        
        public void ConnectTo(ParentTaskNode parentNode, Task childTask)
        {
            TaskNode childNode = FindNode<TaskNode>(childTask);
            if (parentNode != null && childNode != null)
            {
                Edge edge = parentNode.Output.ConnectTo(childNode.Input);
                AddElement(edge);
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (evt.target is GraphView && nodeCreationRequest != null)
            {
                evt.menu.AppendAction("Add Task", a =>
                {
                    nodeCreationRequest(new NodeCreationContext()
                    {
                        screenMousePosition = a.eventInfo.mousePosition
                    });
                }, DropdownMenuAction.AlwaysEnabled);
                evt.menu.AppendSeparator();
            }
            
            if (window.Source != null)
            {
                if (evt.target is RootNode)
                {
                    return;
                }

                if (evt.target is GraphView || evt.target is Node || evt.target is Group)
                    evt.menu.AppendAction("Copy", a => CopySelectionCallback(), a => canCopySelection ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
                if (evt.target is GraphView)
                    evt.menu.AppendAction("Paste", a => PasteCallback(), a => canPaste ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
                if (evt.target is GraphView || evt.target is Node || evt.target is Group || evt.target is Edge)
                {
                    evt.menu.AppendSeparator();
                    evt.menu.AppendAction("Delete", a => DeleteSelectionCallback(AskUser.DontAskUser), a => canDeleteSelection ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
                }

                if (!(evt.target is GraphView) && !(evt.target is Node) && !(evt.target is Group))
                    return;
                evt.menu.AppendSeparator();
                evt.menu.AppendAction("Duplicate", a => DuplicateSelectionCallback(), a => canDuplicateSelection ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled);
                evt.menu.AppendSeparator();
            }
            else
            {
                evt.menu.AppendAction("Add BehaviorTree", a =>
                {
                    window.AddBehavior();
                });
            }
        }

        public override void RemoveFromSelection(ISelectable selectable)
        {
            base.RemoveFromSelection(selectable);
            if (selection.Count == 0)
            {
                onNodeUnselected(null);
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> results = ports.ToList();
            results.RemoveAll(port => startPort.node == port.node ||
                                      startPort.direction == port.direction ||
                                      startPort.portType != port.portType);
            return results;
        }

        private void AddDefaultManipulator()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());
            styleSheets.Add(EditorBehaviorUtility.Load<StyleSheet>("Styles/BehaviorWindow"));
        }

        private void RegisterCreationRequest()
        {
            MenuWindowProvider provider = ScriptableObject.CreateInstance<MenuWindowProvider>();
            provider.Init(window);
            nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), provider);
            };
        }

        private void RegisterCopyAndPaste()
        {
            string copyName = "BehaviorTree Copy Tasks";
            serializeGraphElements += elements =>
            {
                List<Task> tasks = new List<Task>();
                foreach (GraphElement element in elements)
                {
                    if (element is not TaskNode taskNode)
                    {
                        continue;
                    }

                    if (element is RootNode)
                    {
                        continue;
                    }

                    tasks.Add(taskNode.Task);
                }

                copyTasks.Clear();
                window.CopyTasks(tasks, copyTasks);
                return copyName;
            };

            unserializeAndPaste += (operationName, data) =>
            {
                if (data != copyName)
                {
                    return;
                }

                if (copyTasks.Count == 0)
                {
                    return;
                }

                window.UndoRecord("BehaviorTree Copy Tasks");
                List<Task> newTasks = new List<Task>();
                window.CopyTasks(copyTasks, newTasks);
                foreach (Task task in newTasks)
                {
                    task.graphPosition.position += new Vector2(10, 10);
                    window.Source.Tasks.Add(task);
                }

                ClearSelection();
                foreach (Task task in newTasks)
                {
                    TaskNode taskNode = window.CreateNode(task);
                    AddElement(taskNode);
                }

                foreach (Task task in newTasks)
                {
                    if (task is ParentTask parentTask)
                    {
                        ParentTaskNode parentNode = FindNode<ParentTaskNode>(parentTask);
                        foreach (Task childTask in parentTask.Children)
                        {
                            ConnectTo(parentNode, childTask);
                        }
                    }
                }

                foreach (Task task in newTasks)
                {
                    TaskNode taskNode = FindNode<TaskNode>(task);
                    taskNode.Restore();
                    AddToSelection(taskNode);
                }
            };
        }

        private void UpdateViewTransform()
        {
            if (EditorBehaviorUtility.TryGetGuid(Behavior.Object, out string guid))
            {
                Vector3 pos = EditorBehaviorUtility.GetPrefsVector3($"BehaviorDesigner.ViewTransform.Position.{guid}", Vector3.zero);
                Vector3 scl = EditorBehaviorUtility.GetPrefsVector3($"BehaviorDesigner.ViewTransform.Scale.{guid}", Vector3.one);
                UpdateViewTransform(pos, scl);
            }
        }

        private void OnViewTransformChanged(GraphView graphView)
        {
            if (EditorBehaviorUtility.TryGetGuid(Behavior.Object, out string guid))
            {
                EditorBehaviorUtility.SetPrefsVector3($"BehaviorDesigner.ViewTransform.Position.{guid}", viewTransform.position);
                EditorBehaviorUtility.SetPrefsVector3($"BehaviorDesigner.ViewTransform.Scale.{guid}", viewTransform.scale);
            }
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                window.UndoRecord("BehaviorTree Delete Task");
                foreach (GraphElement element in graphViewChange.elementsToRemove)
                {
                    if (element is TaskNode taskNode)
                    {
                        Source.Tasks.Remove(taskNode.Task);
                    }

                    if (element is Edge edge)
                    {
                        ParentTaskNode parentNode = (ParentTaskNode) edge.output.node;
                        TaskNode childNode = (TaskNode) edge.input.node;
                        parentNode.ParentTask.Children.Remove(childNode.Task);
                    }
                }
            }

            if (graphViewChange.edgesToCreate != null)
            {
                window.UndoRecord("BehaviorTree Add Child");
                foreach (Edge edge in graphViewChange.edgesToCreate)
                {
                    ParentTaskNode parentNode = (ParentTaskNode) edge.output.node;
                    TaskNode childNode = (TaskNode) edge.input.node;
                    parentNode.ParentTask.Children.Add(childNode.Task);
                }

                UpdateChildOrder();
            }

            if (graphViewChange.movedElements != null)
            {
                window.UndoRecord("BehaviorTree Update Position");
                foreach (GraphElement element in graphViewChange.movedElements)
                {
                    if (element is TaskNode taskNode)
                    {
                        taskNode.Task.graphPosition = taskNode.GetPosition();
                    }
                }

                UpdateChildOrder();
            }

            return graphViewChange;
        }

        private void UpdateChildOrder()
        {
            foreach (Task task in Source.Tasks)
            {
                if (task is ParentTask parentTask)
                {
                    parentTask.Children.Sort
                    (
                        (c1, c2) => (int) (c1.graphPosition.position.x - c2.graphPosition.position.x)
                    );
                }
            }
        }
    }
}