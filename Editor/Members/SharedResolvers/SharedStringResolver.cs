using System.Reflection;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SharedStringField : SharedVariableField<TextField, SharedString, string>
    {
        public SharedStringField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override TextField CreateEditorField()
        {
            return new TextField();
        }
    }

    public class SharedStringResolver : FieldResolver<SharedStringField, SharedString>
    {
        public SharedStringResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedStringField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedStringField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedString);
        }
    }
}