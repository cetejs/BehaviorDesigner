using System.Reflection;
using UnityEditor.UIElements;

namespace BehaviorDesigner.Editor
{
    public class IntResolver : FieldResolver<IntegerField, int>
    {
        public IntResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override IntegerField CreateEditorField(FieldInfo fieldInfo)
        {
            return new IntegerField(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(int);
        }
    }
}