using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class MenuWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private static List<SearchTreeEntry> entries;
        private List<Type> taskTypes;
        private Dictionary<string, List<Type>> taskGroups;
        private BehaviorWindow window;
        private Texture2D taskIcon;

        public void Init(BehaviorWindow window)
        {
            this.window = window;
            taskIcon = Texture2D.blackTexture;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            CollectAllEntries();
            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            window.RegisterUndo("Add Task");
            Type type = searchTreeEntry.userData as Type;
            Task task = Activator.CreateInstance(type) as Task;
            task.Id = window.Source.NewTaskId();
            TaskNode node = window.CreateNode(task);
            Vector2 worldMousePos = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent, context.screenMousePosition - window.position.position);
            Vector2 localMousePos = window.View.contentViewContainer.WorldToLocal(worldMousePos);
            node.SetPosition(new Rect(localMousePos, new Vector2(100f, 100f)));
            window.View.AddToSelection(node);
            window.View.AddElement(node);
            window.Save();
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

                    TaskCategoryAttribute attribute = type.GetCustomAttribute<TaskCategoryAttribute>();
                    if (attribute != null)
                    {
                        if (!taskGroups.TryGetValue(attribute.category, out List<Type> types))
                        {
                            types = new List<Type>();
                            taskGroups.Add(attribute.category, types);
                        }

                        types.Add(type);
                    }
                    else
                    {
                        taskTypes.Add(type);
                    }
                }
            }

            entries.Add(new SearchTreeGroupEntry(new GUIContent("Add Task")));
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
                    entries.Add(new SearchTreeEntry(new GUIContent(ObjectNames.NicifyVariableName(taskType.Name), taskIcon))
                    {
                        level = 2,
                        userData = taskType
                    });
                }
            }

            foreach (string category in taskGroups.Keys)
            {
                bool hasGroup = false;
                foreach (Type type in taskGroups[category])
                {
                    if (type.IsSubclassOf(typeof(T)))
                    {
                        if (!hasGroup)
                        {
                            entries.Add(new SearchTreeGroupEntry(new GUIContent(category), 2));
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

                        entries.Add(new SearchTreeEntry(new GUIContent(title, taskIcon))
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