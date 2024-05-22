using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner
{
    public class MenuWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private static List<SearchTreeEntry> entries;
        private List<Type> taskTypes;
        private Dictionary<string, List<Type>> taskGroups;
        private BehaviorWindow window;

        public void Init(BehaviorWindow window)
        {
            this.window = window;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            CollectAllEntries();
            entries[0].content.text = "Add Task";
            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Type type = searchTreeEntry.userData as Type;
            if (window.Behavior == null)
            {
                window.AddBehavior();
                Root root = window.Source.root;
                TaskNode node = window.CreateNode(type, root.graphPosition);
                node.Task.graphPosition.y += 150f;
                node.Restore();
                root.Children.Add(node.Task);
                window.View.AddElement(node);
                window.View.ConnectTo(root, node.Task);
            }
            else
            {
                Vector2 worldMousePos = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent, context.screenMousePosition - window.position.position);
                Vector2 localMousePos = window.View.contentViewContainer.WorldToLocal(worldMousePos);
                Rect newPos = new Rect(localMousePos, new Vector2(100f, 100f));
                TaskNode node = window.CreateNode(type, newPos);
                node.Restore();
                node.SetPosition(newPos);
                window.View.AddElement(node);
            }

            return true;
        }

        private void CollectAllEntries()
        {
            if (entries != null)
            {
                return;
            }

            entries = new List<SearchTreeEntry>();
            taskTypes = new List<Type>();
            taskGroups = new Dictionary<string, List<Type>>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsAbstract)
                    {
                        continue;
                    }

                    if (!type.IsSubclassOf(typeof(Task)))
                    {
                        continue;
                    }

                    TaskGroupAttribute attribute = type.GetCustomAttribute<TaskGroupAttribute>();
                    if (attribute != null)
                    {
                        if (!taskGroups.TryGetValue(attribute.group, out List<Type> types))
                        {
                            types = new List<Type>();
                            taskGroups.Add(attribute.group, types);
                        }

                        types.Add(type);
                    }
                    else
                    {
                        taskTypes.Add(type);
                    }
                }
            }

            entries.Add(new SearchTreeGroupEntry(new GUIContent()));
            AddTaskGroup<Action>();
            AddTaskGroup<Composite>();
            AddTaskGroup<Conditional>();
            AddTaskGroup<Decorator>();
        }

        private void AddTaskGroup<T>() where T : Task
        {
            entries.Add(new SearchTreeGroupEntry(new GUIContent(typeof(T).Name), 1));
            foreach (Type taskType in taskTypes)
            {
                if (taskType.IsSubclassOf(typeof(T)))
                {
                    entries.Add(new SearchTreeEntry(new GUIContent(ObjectNames.NicifyVariableName(taskType.Name), Texture2D.blackTexture))
                    {
                        level = 2,
                        userData = taskType
                    });
                }
            }

            foreach (string group in taskGroups.Keys)
            {
                bool hasGroup = false;
                foreach (Type type in taskGroups[group])
                {
                    if (type.IsSubclassOf(typeof(T)))
                    {
                        if (!hasGroup)
                        {
                            entries.Add(new SearchTreeGroupEntry(new GUIContent(group), 2));
                            hasGroup = true;
                        }

                        string title;
                        TaskNameAttribute attribute = type.GetCustomAttribute<TaskNameAttribute>();
                        if (attribute != null)
                        {
                            title = ObjectNames.NicifyVariableName(attribute.name);
                        }
                        else
                        {
                            title = ObjectNames.NicifyVariableName(type.Name);
                        }

                        entries.Add(new SearchTreeEntry(new GUIContent(title, Texture2D.blackTexture))
                        {
                            level = 3,
                            userData = type
                        });
                    }
                }
            }
        }
    }
}