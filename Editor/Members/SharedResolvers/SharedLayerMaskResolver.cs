using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    public class SharedLayerMaskField : SharedVariableField<LayerMaskField, SharedLayerMask, int>
    {
        public SharedLayerMaskField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override LayerMaskField CreateEditorField()
        {
            return new LayerMaskField();
        }

        protected override void DrawValue(object obj)
        {
            editorField.value = (LayerMask)obj;
        }

        protected override void SaveValue(int obj)
        {
            value.Value = obj;
        }
    }

    public class SharedLayerMaskResolver : FieldResolver<SharedLayerMaskField, SharedLayerMask>
    {
        public SharedLayerMaskResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedLayerMaskField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedLayerMaskField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedLayerMask);
        }
    }
}