using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    public class ReplaceMenuWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private Dictionary<Type, List<SearchTreeEntry>> entries;
        private BehaviorWindow window;
        private TaskNode node;
        private Texture2D taskIcon;

        public void Init(BehaviorWindow window, TaskNode node)
        {
            this.window = window;
            this.node = node;
            taskIcon = Texture2D.blackTexture;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            Type type = node.Task.GetType();
            while (type != null &&
                   type != typeof(Action) &&
                   type != typeof(Composite) &&
                   type != typeof(Conditional) &&
                   type != typeof(Decorator))
            {
                type = type.BaseType;
            }

            return GetGroupEntries(type);
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            window.RegisterUndo("Replace TaskNode");
            Type type = searchTreeEntry.userData as Type;
            Task task = Activator.CreateInstance(type) as Task;
            task.Id = window.Source.NewTaskId();
            node.Replace(task);
            window.Save();
            return true;
        }

        private List<SearchTreeEntry> GetGroupEntries(Type type)
        {
            if (entries == null)
            {
                entries = new Dictionary<Type, List<SearchTreeEntry>>();
                CollectAllEntries();
            }

            return entries[type];
        }

        private void CollectAllEntries()
        {
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

                    CollectGroupEntries<Action>(type);
                    CollectGroupEntries<Composite>(type);
                    CollectGroupEntries<Conditional>(type);
                    CollectGroupEntries<Decorator>(type);
                }
            }
        }

        private void CollectGroupEntries<T>(Type type) where T : Task
        {
            Type baseType = typeof(T);
            if (type.IsSubclassOf(baseType))
            {
                if (!entries.TryGetValue(baseType, out List<SearchTreeEntry> list))
                {
                    list = new List<SearchTreeEntry>();
                    list.Add(new SearchTreeGroupEntry(new GUIContent($"Select {baseType.Name}")));
                    entries.Add(baseType, list);
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

                list.Add(new SearchTreeEntry(new GUIContent(title, taskIcon))
                {
                    level = 1,
                    userData = type
                });
            }
        }
    }
}