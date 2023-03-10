using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    public abstract class MenuWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private static List<SearchTreeEntry> entries;
        private List<Type> taskTypes;
        private Dictionary<string, List<Type>> taskGroups;
        protected BehaviorWindow window;
        private Texture2D taskIcon;

        public virtual string Title { get; }

        public virtual void Init(BehaviorWindow window)
        {
            this.window = window;
            taskIcon = Texture2D.blackTexture;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            CollectAllEntries();
            entries[0].content.text = Title;
            return entries;
        }

        public abstract bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context);

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