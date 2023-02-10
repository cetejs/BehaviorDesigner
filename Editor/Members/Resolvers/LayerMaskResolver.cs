using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class LayerMaskResolver : FieldResolver<LayerMaskField, int>
    {
        public LayerMaskResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        public override void Register(SharedVariable variable)
        {
            editorField.RegisterValueChangedCallback(evt =>
            {
                window.RegisterUndo("Update Variable");
                variable.SetValue((LayerMask) evt.newValue);
                window.Save();
            });
        }

        public override void Restore(SharedVariable variable)
        {
            editorField.value = (LayerMask) variable.GetValue();
        }

        public override void Restore(Task task)
        {
            editorField.value = (LayerMask) fieldInfo.GetValue(task);
        }

        public override void Save(Task task)
        {
            fieldInfo.SetValue(task, (LayerMask) editorField.value);
        }

        protected override LayerMaskField CreateEditorField(FieldInfo fieldInfo)
        {
            return new LayerMaskField(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(LayerMask);
        }
    }
}