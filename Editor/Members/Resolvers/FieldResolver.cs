using System;
using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace BehaviorDesigner.Editor
{
    public interface IFieldResolver
    {
        VisualElement EditorField { get; }

        void Register();

        void Register(SharedVariable variable);
        
        void Restore(SharedVariable variable);
        
        void Restore(Task task);

        void Save(Task task);
    }

    public abstract class FieldResolver<TField, TFieldValue> : IFieldResolver where TField : BaseField<TFieldValue>
    {
        protected readonly FieldInfo fieldInfo;
        protected TField editorField;
        protected BehaviorWindow window;

        public FieldResolver(FieldInfo fieldInfo, BehaviorWindow window)
        {
            this.fieldInfo = fieldInfo;
            this.window = window;
            editorField = CreateEditorField(fieldInfo);
            editorField.label = ObjectNames.NicifyVariableName(editorField.label);
            if (this.fieldInfo.GetCustomAttribute<FieldReadOnlyAttribute>() != null)
            {
                editorField.SetEnabled(false);
            }
        }

        public VisualElement EditorField
        {
            get { return editorField; }
        }

        public virtual void Register()
        {
            editorField.RegisterValueChangedCallback(evt =>
            {
                window.RegisterUndo("Update FieldValue");
                window.Save();
            });
        }

        public virtual void Register(SharedVariable variable)
        {
            editorField.RegisterValueChangedCallback(evt =>
            {
                window.RegisterUndo("Update Variable");
                variable.SetValue(evt.newValue);
                window.Save();
            });
        }
        
        public virtual void Restore(SharedVariable variable)
        {
            editorField.value = (TFieldValue) variable.GetValue();
        }

        public virtual void Restore(Task task)
        {
            object value = fieldInfo.GetValue(task);
            if (value == null && !typeof(Object).IsAssignableFrom(fieldInfo.FieldType) && !fieldInfo.FieldType.IsPrimitive)
            {
                value = Activator.CreateInstance(fieldInfo.FieldType);
            }

            editorField.value = (TFieldValue) value;
        }

        public virtual void Save(Task task)
        {
            fieldInfo.SetValue(task, editorField.value);
        }

        protected abstract TField CreateEditorField(FieldInfo fieldInfo);
    }
}