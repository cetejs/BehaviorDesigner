using System.Reflection;
using UnityEditor.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SharedLongField : SharedVariableField<LongField, SharedLong, long>
    {
        public SharedLongField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override LongField CreateEditorField()
        {
            return new LongField();
        }
    }

    public class SharedLongResolver : FieldResolver<SharedLongField, SharedLong>
    {
        public SharedLongResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedLongField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedLongField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedLong);
        }
    }
}