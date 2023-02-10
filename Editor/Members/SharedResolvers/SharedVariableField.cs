using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public abstract class SharedVariableField<TField, TFieldValue, TFieldType> : BaseField<TFieldValue>
        where TField : BaseField<TFieldType> where TFieldValue : SharedVariable
    {
        protected FieldInfo fieldInfo;
        protected TField editorField;
        protected Toggle toggle;
        protected PopupField<string> sharedNameField;
        protected BehaviorWindow window;
        protected List<string> sharedNames;
        protected string fieldName;

        public SharedVariableField(FieldInfo fieldInfo, BehaviorWindow window) : base(null, null)
        {
            this.fieldInfo = fieldInfo;
            this.window = window;
            sharedNames = new List<string>();
            fieldName = ObjectNames.NicifyVariableName(fieldInfo.Name);

            editorField = CreateEditorField();
            editorField.RegisterValueChangedCallback(evt =>
            {
                window.RegisterUndo("Update FieldValue");
                SaveValue(evt.newValue);
                window.Save();
            });

            toggle = new Toggle();
            toggle.RegisterValueChangedCallback(evt =>
            {
                window.RegisterUndo("Update SharedState");
                value.IsShared = evt.newValue;
                window.Save();
                ChangeSharedState();
            });

            sharedNameField = new PopupField<string>();
            sharedNameField.RegisterValueChangedCallback(evt =>
            {
                window.RegisterUndo("Update SharedName");
                value.Name = evt.newValue;
                window.Save();
            });

            window.RegisterVariableChangedCallback(list =>
            {
                CollectSharedNames();
            });
        }

        public override TFieldValue value
        {
            get { return base.value; }
            set
            {
                base.value = value;
                RequiredField();
                ChangeSharedState();
                UpdateEditorValue();
            }
        }
        
        protected abstract TField CreateEditorField();

        protected virtual void DrawValue(object obj)
        {
            editorField.value = (TFieldType)obj;
        }

        protected virtual void SaveValue(TFieldType obj)
        {
            value.SetValue(obj);
        }

        protected virtual void SetFieldName(string name)
        {
            editorField.label = name;
        }

        private void RequiredField()
        {
            if (fieldInfo.GetCustomAttribute<RequiredFieldAttribute>() != null)
            {
                value.IsShared = true;
                toggle.SetEnabled(false);
            }
        }

        private void ChangeSharedState()
        {
            if (value.IsShared)
            {
                Clear();
                CollectSharedNames();
                Add(labelElement);
                Add(sharedNameField);
                Add(toggle);
                labelElement.text = fieldName;
            }
            else
            {
                Clear();
                Add(editorField);
                Add(toggle);
                SetFieldName(fieldName);
            }
        }

        private void UpdateEditorValue()
        {
            DrawValue(value.GetValue());
            sharedNameField.value = value.Name;
            toggle.value = value.IsShared;
        }

        private void CollectSharedNames()
        {
            if (value == null || !value.IsShared)
            {
                return;
            }

            sharedNames.Clear();
            IEnumerable<SharedVariable> variables = window.Source.GetVariables();
            foreach (SharedVariable variable in variables)
            {
                if (variable != null && variable.GetType() == fieldInfo.FieldType)
                {
                    sharedNames.Add(variable.Name);
                }
            }

            if (sharedNames.Count == 0 || !sharedNames.Contains(value.Name))
            {
                sharedNameField.value = value.Name = $"Invalid {value.GetType().Name.Replace("Shared", "")}";
            }

            sharedNameField.choices = sharedNames;
        }
    }
}