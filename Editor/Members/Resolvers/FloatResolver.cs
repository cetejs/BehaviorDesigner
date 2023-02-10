using System.Reflection;
using UnityEditor.UIElements;

namespace BehaviorDesigner.Editor
{
    public class FloatResolver : FieldResolver<FloatField, float>
    {
        public FloatResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override FloatField CreateEditorField(FieldInfo fieldInfo)
        {
            return new FloatField(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(float);
        }
    }
}