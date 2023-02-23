using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BehaviorDesigner
{
    [Serializable]
    public class BehaviorSource
    {
        public string behaviorName = "Behavior";
        public int group;
        public string behaviorDescription;
        [SerializeField]
        private string rootJson;
        [SerializeField]
        private string detachedTasksJson;
        [SerializeField]
        private string variablesJson;
        [HideInInspector]
        [SerializeField]
        private int version;
        [SerializeField]
        private int guidCount;
        [SerializeField]
        private List<UnityObject> unityObjects;
        private Root root;
        private List<Task> detachedTasks;
        private List<SharedVariable> variables;
        private Dictionary<string, int> sharedVariableIndex;
        private JsonSerialization jsonSerialization;

        public Root Root
        {
            get { return root; }
        }

        public int Version
        {
            get { return version; }
            set { version = value; }
        }

        public List<SharedVariable> GetAllVariables()
        {
            return variables;
        }

        public SharedVariable GetVariable(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            if (sharedVariableIndex.TryGetValue(name, out int index))
            {
                return variables[index];
            }

            return null;
        }

        public SharedVariable<T> GetVariable<T>(string name)
        {
            if (sharedVariableIndex.TryGetValue(name, out int index))
            {
                return variables[index] as SharedVariable<T>;
            }

            return null;
        }

        public void Load()
        {
            if (Application.isPlaying && root != null)
            {
                return;
            }
            
            if (unityObjects == null)
            {
                unityObjects = new List<UnityObject>();
            }

            jsonSerialization = new JsonSerialization();
            jsonSerialization.LoadUnityObjects(ref unityObjects);
            if (string.IsNullOrEmpty(rootJson))
            {
                root = new Root();
            }
            else
            {
                root = jsonSerialization.ToTask(rootJson) as Root;
            }

            if (Application.isEditor)
            {
                if (string.IsNullOrEmpty(detachedTasksJson))
                {
                    detachedTasks = new List<Task>();
                }
                else
                {
                    detachedTasks = jsonSerialization.ToTaskArray(detachedTasksJson);
                }
            }  
            
            if (string.IsNullOrEmpty(variablesJson))
            {
                variables = new List<SharedVariable>();
            }
            else
            {
                variables = jsonSerialization.ToArray<SharedVariable>(variablesJson);
            }

            UpdateVariableList();
        }

        public void ReloadVariables()
        {
            if (string.IsNullOrEmpty(variablesJson))
            {
                variables = new List<SharedVariable>();
            }
            else
            {
                variables = jsonSerialization.ToArray<SharedVariable>(variablesJson);
            }

            UpdateVariableList();
        }

        public void Save()
        {
            unityObjects.Clear();
            jsonSerialization.LoadUnityObjects(ref unityObjects);
            rootJson = jsonSerialization.FromTask(root);
            detachedTasksJson = jsonSerialization.FromTaskArray(detachedTasks);
            variablesJson = jsonSerialization.FromArray(variables);
            jsonSerialization.SaveUnityObjects(ref unityObjects);
        }

        public void Save(BehaviorSource other)
        {
            Save();
            if (other == this)
            {
                return;
            }

            other.rootJson = rootJson;
            other.detachedTasksJson = detachedTasksJson;
            other.variablesJson = variablesJson;
        }

        public string FromTaskArray(List<Task> tasks, HashSet<Task> checkChildren = null)
        {
            return jsonSerialization.FromTaskArray(tasks, checkChildren);
        }

        public void ToTaskArray(string json, List<Task> tasks)
        {
            jsonSerialization.ToTaskArray(json, tasks);
        }

        public void AddDetachedTask(Task task)
        {
            detachedTasks.Add(task);
        }

        public void RemoveDetachedTask(Task task)
        {
            detachedTasks.Remove(task);
        }

        public void UpdateDetachedTasks()
        {
            for (int i = 0; i < detachedTasks.Count; i++)
            {
                if (detachedTasks[i] == null)
                {
                    detachedTasks.RemoveAt(i);
                }
                else
                {
                    UpdateDetachedTasks(detachedTasks[i]);
                }
            }
        }

        public IEnumerable<Task> GetDetachedTasks()
        {
            return detachedTasks;
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

        public void AddVariable(SharedVariable variable)
        {
            if (!sharedVariableIndex.TryGetValue(variable.Name, out int index))
            {
                sharedVariableIndex.Add(variable.Name, variables.Count);
                variables.Add(variable);
            }
        }

        public void RemoveVariable(string name)
        {
            if (sharedVariableIndex.TryGetValue(name, out int index))
            {
                variables.RemoveAt(index);
            }

            UpdateVariableList();
        }

        public bool ContainsVariable(string name)
        {
            return sharedVariableIndex.ContainsKey(name);
        }

        public void RenameVariable(string oldName, string newName)
        {
            if (sharedVariableIndex.ContainsKey(newName))
            {
                return;
            }

            if (sharedVariableIndex.TryGetValue(oldName, out int index))
            {
                variables[index].Name = newName;
                sharedVariableIndex.Remove(oldName);
                sharedVariableIndex.Add(newName, index);
            }
        }

        public bool CanMoveVariable(string name, bool isMoveDown)
        {
            if (sharedVariableIndex.TryGetValue(name, out int index))
            {
                if (isMoveDown && index == variables.Count - 1)
                {
                    return false;
                }

                if (!isMoveDown && index == 0)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public void MoveVariable(string name, bool isMoveDown)
        {
            if (!CanMoveVariable(name, isMoveDown))
            {
                return;
            }

            if (sharedVariableIndex.TryGetValue(name, out int index))
            {
                int moveIndex = isMoveDown ? index + 1 : index - 1;
                SharedVariable variable = variables[index];
                variables.RemoveAt(index);
                variables.Insert(moveIndex, variable);
                UpdateVariableList();
            }
        }

        public void ChangeVariable(string name, SharedVariable newVariable)
        {
            if (sharedVariableIndex.TryGetValue(name, out int index))
            {
                newVariable.Name = name;
                variables.RemoveAt(index);
                variables.Add(newVariable);
                UpdateVariableList();
            }
        }

        public void ClearVariables()
        {
            variables.Clear();
            UpdateVariableList();
        }

        public void UpdateVariableList()
        {
            if (sharedVariableIndex == null)
            {
                sharedVariableIndex = new Dictionary<string, int>();
            }
            else
            {
                sharedVariableIndex.Clear();
            }

            for (int i = 0; i < variables.Count; i++)
            {
                sharedVariableIndex.Add(variables[i].Name, i);
            }
        }

        public IEnumerable<SharedVariable> GetVariables()
        {
            return variables;
        }

        public void ClearDetachedTasks()
        {
            detachedTasks.Clear();
        }

        public int NewTaskGuid()
        {
            return ++guidCount;
        }

        private void UpdateDetachedTasks(Task task)
        {
            if (task is not ParentTask parentTask)
            {
                return;
            }

            for (int i = 0; i < parentTask.Children.Count; i++)
            {
                if (parentTask.Children[i] == null)
                {
                    parentTask.Children.RemoveAt(i);
                }
                else
                {
                    UpdateDetachedTasks(parentTask.Children[i]);
                }
            }
        }
    }
}