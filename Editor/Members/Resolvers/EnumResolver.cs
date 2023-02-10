using System;
using System.Reflection;
using UnityEditor.UIElements;

namespace BehaviorDesigner.Editor
{
    public class EnumResolver : FieldResolver<EnumField, Enum>
    {
        public EnumResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override EnumField CreateEditorField(FieldInfo fieldInfo)
        {
            Array array = Enum.GetValues(fieldInfo.FieldType);
            return new EnumField(fieldInfo.Name, (Enum) array.GetValue(0));
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType.IsEnum;
        }
    }
}