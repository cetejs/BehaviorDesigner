using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class StringResolver : FieldResolver<TextField, string>
    {
        public StringResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override TextField CreateEditorField(FieldInfo fieldInfo)
        {
            return new TextField(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(string);
        }
    }
}