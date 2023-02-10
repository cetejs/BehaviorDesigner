using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SharedTransformField : SharedVariableField<ObjectField, SharedTransform, Object>
    {
        public SharedTransformField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
            this.fieldInfo = fieldInfo;
        }

        protected override ObjectField CreateEditorField()
        {
            ObjectField field = new ObjectField();
            field.objectType = typeof(Transform);
            return field;
        }
    }

    public class SharedTransformResolver : FieldResolver<SharedTransformField, SharedTransform>
    {
        public SharedTransformResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedTransformField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedTransformField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedTransform);
        }
    }
}