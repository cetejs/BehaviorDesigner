using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class VariablesView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<VariablesView, UxmlTraits> {}

        private BehaviorWindow window;
        private ScrollView scrollView;
        private ToolbarMenu toolbarMenu;
        private TextField nameInput;
        private DropdownField typeDp;
        private Button addBtn;
        private Dictionary<string, IFieldResolver> resolvers;
        private static List<PriorityType> sharedVariableTypes;
        private List<string> variableTypeChoices;
        public event Action<IEnumerable<SharedVariable>> onVariableChanged;

        public BehaviorWindow Window
        {
            get { return window; }
        }

        public List<string> VariableTypeChoices
        {
            get { return variableTypeChoices; }
        }

        public void Init(BehaviorWindow window)
        {
            this.window = window;
            styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/VariablesView"));
            scrollView = this.Q<ScrollView>();
            resolvers = new Dictionary<string, IFieldResolver>();
            RegisterMenu();
        }
        
        public void Update()
        {
            if (Application.isPlaying)
            {
                foreach (SharedVariable variable in window.Source.GetVariables())
                {
                    resolvers[variable.Name].Restore(variable);
                }
            }
        }

        public void Restore()
        {
            scrollView.Clear();
            resolvers.Clear();
            window.Source.UpdateVariableList();
            foreach (SharedVariable variable in window.Source.GetVariables())
            {
                AddProperty(variable);
            }

            UpdateStateVariables();
            onVariableChanged?.Invoke(window.Source.GetVariables());
        }

        private void RegisterMenu()
        {
            CollectAllSharedVariables();
            variableTypeChoices = new List<string>(sharedVariableTypes.Count);
            foreach (PriorityType priorityType in sharedVariableTypes)
            {
                variableTypeChoices.Add(priorityType.type.Name.Replace("Shared", ""));
            }

            toolbarMenu = parent.Q<ToolbarMenu>();
            toolbarMenu.menu.AppendAction("Delete All", action =>
            {
                DeleteAllVariables();
            });

            nameInput = this.Q<TextField>("name-input");
            nameInput.RegisterValueChangedCallback(evt =>
            {
                UpdateAddState();
            });

            typeDp = this.Q<DropdownField>("type-dp");
            typeDp.choices = variableTypeChoices;
            typeDp.index = 0;

            addBtn = this.Q<Button>("add-btn");
            addBtn.SetEnabled(false);
            addBtn.clickable.clicked += () =>
            {
                AddVariable();
            };
        }

        private void AddProperty(SharedVariable variable, int index = -1)
        {
            Type type = variable.GetType();
            FieldInfo info = type.GetField("value", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            IFieldResolver resolver = window.CreateField(info);
            VariableField variableField = new VariableField(this, variable, resolver.EditorField);
            variableField.RegisterCallback(RenameVariable, ChangeVariable, MoveVariable, DeleteVariable);
            resolver.Register(variable);
            resolver.Restore(variable);
            scrollView.Insert(index > 0 ? index : scrollView.childCount, variableField);
            resolvers.Add(variable.Name, resolver);
        }

        private void UpdateAddState()
        {
            bool canAdd = !string.IsNullOrEmpty(nameInput.value) && !window.Source.ContainsVariable(nameInput.value);
            addBtn.SetEnabled(canAdd);
        }

        private void AddVariable()
        {
            Type type = sharedVariableTypes[typeDp.index].type;
            SharedVariable variable = (SharedVariable) Activator.CreateInstance(type);
            variable.Name = nameInput.value;
            addBtn.SetEnabled(false);
            AddProperty(variable);
            window.RegisterUndo("Add Variable");
            window.Source.AddVariable(variable);
            window.Save();
            UpdateStateVariables();
            onVariableChanged?.Invoke(window.Source.GetVariables());
        }

        private bool RenameVariable(string newValue, VariableField field)
        {
            string oldValue = field.Value.Name;
            if (!string.IsNullOrEmpty(newValue) && !window.Source.ContainsVariable(newValue))
            {
                resolvers.Add(newValue, resolvers[oldValue]);
                resolvers.Remove(oldValue);
                window.RegisterUndo("Rename Variable");
                window.Source.RenameVariable(oldValue, newValue);
                window.Save();
                UpdateAddState();
                UpdateStateVariables();
                onVariableChanged?.Invoke(window.Source.GetVariables());
                return true;
            }

            return false;
        }

        private void ChangeVariable(int typeIndex, VariableField field)
        {
            string variableName = field.Value.Name;
            int index = scrollView.IndexOf(field);
            scrollView.RemoveAt(index);
            resolvers.Remove(variableName);
            Type type = sharedVariableTypes[typeIndex].type;
            SharedVariable variable = (SharedVariable) Activator.CreateInstance(type);
            variable.Name = variableName;
            AddProperty(variable, index);
            window.RegisterUndo("Change Variable");
            window.Source.ChangeVariable(variableName, variable);
            window.Save();
            UpdateStateVariables();
            onVariableChanged?.Invoke(window.Source.GetVariables());
        }

        private void MoveVariable(VariableField field, bool isMoveDown)
        {
            int index = scrollView.IndexOf(field);
            int moveIndex = isMoveDown ? index + 1 : index - 1;
            scrollView.RemoveAt(index);
            scrollView.Insert(moveIndex, field);
            window.RegisterUndo("Move Variable");
            window.Source.MoveVariable(field.Value.Name, isMoveDown);
            window.Save();
            UpdateStateVariables();
        }

        private void DeleteVariable(VariableField field)
        {
            EditorPrefs.DeleteKey($"BehaviorDesign.Expanded.{window.BehaviorFileId}.{field.Value.Name}");
            scrollView.Remove(field);
            resolvers.Remove(field.Value.Name);
            window.RegisterUndo("Delete Variable");
            window.Source.RemoveVariable(field.Value.Name);
            window.Save();
            UpdateAddState();
            UpdateStateVariables();
            onVariableChanged?.Invoke(window.Source.GetVariables());
        }

        private void DeleteAllVariables()
        {
            if (resolvers.Count == 0)
            {
                return;
            }

            foreach (SharedVariable variable in window.Source.GetVariables())
            {
                EditorPrefs.DeleteKey($"BehaviorDesign.Expanded.{window.BehaviorFileId}.{variable.Name}");
            }

            scrollView.Clear();
            resolvers.Clear();
            window.RegisterUndo("DeleteAll Variables");
            window.Source.ClearVariables();
            window.Save();
        }

        private void UpdateStateVariables()
        {
            foreach (VisualElement child in scrollView.Children())
            {
                VariableField field = child as VariableField;
                field?.UpdateBtnState();
            }
            
            toolbarMenu.SetEnabled(scrollView.childCount > 0);
        }

        private void CollectAllSharedVariables()
        {
            if (sharedVariableTypes != null)
            {
                return;
            }

            sharedVariableTypes = new List<PriorityType>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsAbstract)
                    {
                        continue;
                    }

                    if (!type.IsSubclassOf(typeof(SharedVariable)))
                    {
                        continue;
                    }

                    VariablePriorityAttribute attribute = type.GetCustomAttribute<VariablePriorityAttribute>();
                    int priority = attribute == null ? 0 : attribute.priority;

                    int index = sharedVariableTypes.Count;
                    for (int i = sharedVariableTypes.Count - 1; i >= 0; i--)
                    {
                        if (priority >= sharedVariableTypes[i].priority)
                        {
                            index = i + 1;
                            break;
                        }
                    }

                    sharedVariableTypes.Insert(index, new PriorityType()
                    {
                        priority = priority,
                        type = type
                    });
                }
            }
        }

        private struct PriorityType
        {
            public int priority;
            public Type type;
        }
    }
}