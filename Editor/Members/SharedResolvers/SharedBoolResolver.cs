using System.Reflection;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SharedBoolField : SharedVariableField<Toggle, SharedBool, bool>
    {
        public SharedBoolField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override Toggle CreateEditorField()
        {
            return new Toggle();
        }
    }

    public class SharedBoolResolver : FieldResolver<SharedBoolField, SharedBool>
    {
        public SharedBoolResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedBoolField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedBoolField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedBool);
        }
    }
}