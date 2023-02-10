using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SharedFloatField : SharedVariableField<FloatField, SharedFloat, float>
    {
        public SharedFloatField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override FloatField CreateEditorField()
        {
            return new FloatField();
        }
    }

    public class SharedFloatResolver : FieldResolver<SharedFloatField, SharedFloat>
    {
        public SharedFloatResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedFloatField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedFloatField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedFloat);
        }
    }
}