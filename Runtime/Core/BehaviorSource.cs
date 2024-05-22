using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    public class BehaviorSource : ISerializationCallbackReceiver
    {
        public string behaviorName = "Behavior";
        [TextArea]
        public string behaviorDescription;
        [HideInInspector]
        public int serializedVersion;
        [SerializeReference]
        public Root root;
        [SerializeReference]
        private List<Task> tasks = new List<Task>();
        [SerializeReference]
        private List<SharedVariable> sharedVariables = new List<SharedVariable>();
        private Dictionary<string, int> sharedVariableIndex = new Dictionary<string, int>();

        public List<Task> Tasks
        {
            get { return tasks; }
        }

        public Task FindTask(string guid)
        {
            foreach (Task task in tasks)
            {
                if (task.Guid == guid)
                {
                    return task;
                }
            }

            return null;
        }

        public List<SharedVariable> Variables
        {
            get { return sharedVariables; }
        }

        public SharedVariable GetVariable(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            if (sharedVariableIndex.TryGetValue(name, out int index))
            {
                return sharedVariables[index];
            }

            return null;
        }

        public SharedVariable<T> GetVariable<T>(string name)
        {
            if (sharedVariableIndex.TryGetValue(name, out int index))
            {
                return sharedVariables[index] as SharedVariable<T>;
            }

            return null;
        }

        public void BindVariables(Task task)
        {
            Type type = task.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                if (!field.IsPublic && field.GetCustomAttribute<SerializeField>() == null)
                {
                    continue;
                }

                if (field.FieldType.IsSubclassOf(typeof(SharedVariable)))
                {
                    if (field.GetValue(task) is not SharedVariable value)
                    {
                        continue;
                    }

                    SharedVariable variable = GetVariable(value.Name);
                    if (!value.IsShared)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(value.Name))
                    {
                        continue;
                    }

                    if (variable != null)
                    {
                        value.Bind(variable);
                    }
                    else
                    {
                        Debug.LogError($"{task.GetType().Name} shared variable {value.Name} does not exist");
                    }
                }
            }
        }

        public void ReloadVariables(BehaviorSource source)
        {
            for (int i = 0; i < sharedVariables.Count; i++)
            {
                sharedVariables[i].SetValue(source.sharedVariables[i].GetValue());
            }

            UpdateVariables();
        }

        public void UpdateVariables()
        {
            sharedVariableIndex.Clear();
            for (int i = 0; i < sharedVariables.Count; i++)
            {
                sharedVariableIndex.Add(sharedVariables[i].Name, i);
            }
        }

        public void AddVariable(SharedVariable variable)
        {
            if (!sharedVariableIndex.TryGetValue(variable.Name, out int index))
            {
                sharedVariableIndex.Add(variable.Name, sharedVariables.Count);
                sharedVariables.Add(variable);
            }
        }

        public void RemoveVariable(int index)
        {
            sharedVariables.RemoveAt(index);
            UpdateVariables();
        }

        public bool ContainsVariable(string name)
        {
            return sharedVariableIndex.ContainsKey(name);
        }

        public void ClearVariables()
        {
            sharedVariables.Clear();
            UpdateVariables();
        }

        public void OnBeforeSerialize()
        {
            if (root == null)
            {
                root = new Root();
                root.Name = nameof(Root);
                root.Guid = Guid.NewGuid().ToString();
                tasks.Add(root);
            }
        }

        public void OnAfterDeserialize()
        {
        }
    }
}