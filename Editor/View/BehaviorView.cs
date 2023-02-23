using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class BehaviorView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviorView, UxmlTraits> { }
        
        private BehaviorWindow window;
        private RootNode rootNode;
        private readonly List<Port> compatiblePorts = new List<Port>();
        private readonly List<Task> copyTasks = new List<Task>();
        private readonly HashSet<Task> checkChildren = new HashSet<Task>();
        private readonly List<int> selections = new List<int>();

        public void Init()
        {
            styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/BehaviorWindow"));
            AddDefaultManipulator();
        }

        public void Init(BehaviorWindow window)
        {
            this.window = window;
            styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/BehaviorWindow"));
            AddDefaultManipulator();
            RegisterCreationRequest();
            RegisterDeleteSelection();
            RegisterCopyAndPaste();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (window)
            {
                if (evt.target is RootNode)
                {
                    return;
                }
                
                base.BuildContextualMenu(evt);
            }
            else
            {
                evt.menu.AppendAction("Add BehaviorTree", action =>
                {
                    if (Selection.activeGameObject)
                    {
                        BehaviorTree behavior =  Selection.activeGameObject.AddComponent<BehaviorTree>();
                        BehaviorWindow.ShowWindow(behavior);
                    }
                    else
                    {
                        BehaviorTree behavior = new GameObject().AddComponent<BehaviorTree>();
                        Selection.activeGameObject = behavior.gameObject;
                    }
                });
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            compatiblePorts.Clear();
            foreach (Port port in ports.ToList())
            {
                if (startPort.node == port.node ||
                    startPort.direction == port.direction ||
                    startPort.portType != port.portType)
                {
                    continue;
                }

                compatiblePorts.Add(port);
            }

            return compatiblePorts;
        }

        public void Restore()
        {
            CollectSelection();
            DeleteElements(graphElements.ToList());
            Root root = window.Behavior.Root;
            rootNode = new RootNode();
            rootNode.Init(root, window);
            AddElement(rootNode);
            RestoreDetachedTasks();
            RestoreSelection();
        }

        public void Save()
        {
            rootNode.Save();
            CollectAllDetachedTasks();
        }

        private void AddDefaultManipulator()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());
        }

        private void CollectAllDetachedTasks()
        {
            window.Source.ClearDetachedTasks();
            foreach (Node node in nodes)
            {
                if (node is not TaskNode taskNode)
                {
                    continue;
                }

                if (node is RootNode)
                {
                    continue;
                }

                if (taskNode.ParentNode != null)
                {
                    continue;
                }

                taskNode.Save();
                window.Source.AddDetachedTask(taskNode.Task);
            }
        }

        private void CollectSelection()
        {
            selections.Clear();
            foreach (ISelectable selectable in selection)
            {
                if (selectable is TaskNode node)
                {
                    selections.Add(node.Task.Guid);
                }
            }
        }

        private void RestoreSelection()
        {
            int lastSelectionId = 0;
            TaskNode lastSelection = null;
            if (selections.Count > 0)
            {
                lastSelectionId = selections[selections.Count -1];
            }

            foreach (GraphElement element in graphElements)
            {
                if (element is TaskNode node && selections.Contains(node.Task.Guid))
                {
                    if (node.Task.Guid == lastSelectionId)
                    {
                        lastSelection = node;
                    }
                    else
                    {
                        AddToSelection(element);
                    }
                }
            }

            if (lastSelection != null)
            {
                AddToSelection(lastSelection);
            }
        }

        private void RestoreDetachedTasks()
        {
            window.Source.UpdateDetachedTasks();
            IEnumerable<Task> tasks = window.Source.GetDetachedTasks();
            foreach (Task task in tasks)
            {
                TaskNode node = window.CreateNode(task);
                node.SetPosition(task.graphPosition);
                AddElement(node);
            }
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

        private void RegisterDeleteSelection()
        {
            deleteSelection += (operationName, user) =>
            {
                window.RegisterUndo("Delete Selection");
                DeleteSelection();
                window.Save();
            };
        }

        private void RegisterCopyAndPaste()
        {
            serializeGraphElements += elements =>
            {
                copyTasks.Clear();
                checkChildren.Clear();
                HashSet<GraphElement> hashElements = (HashSet<GraphElement>) elements;
                foreach (GraphElement element in hashElements)
                {
                    if (element is not TaskNode taskNode)
                    {
                        continue;
                    }

                    if (element is RootNode)
                    {
                        continue;
                    }

                    checkChildren.Add(taskNode.Task);
                    if (taskNode.ParentNode != rootNode &&
                        hashElements.Contains(taskNode.ParentNode))
                    {
                        continue;
                    }

                    copyTasks.Add(taskNode.Task);
                }

                return window.Source.FromTaskArray(copyTasks, checkChildren);
            };

            unserializeAndPaste += (operationName, data) =>
            {
                window.RegisterUndo("Paste TaskNodes");
                window.Source.ToTaskArray(data, copyTasks);
                if (copyTasks.Count > 0)
                {
                    ClearSelection();
                }

                foreach (Task task in copyTasks)
                {
                    TaskNode node = window.CreateNode(task);
                    AddElement(node);
                    node.Deep(tempNode =>
                    {
                        tempNode.Task.Guid = window.Source.NewTaskGuid();
                        tempNode.Task.graphPosition.position += new Vector2(10f, 10f);
                        tempNode.SetPosition(tempNode.Task.graphPosition);
                        AddToSelection(tempNode);
                    });
                }
                
                window.Save();
            };
        }
    }
}