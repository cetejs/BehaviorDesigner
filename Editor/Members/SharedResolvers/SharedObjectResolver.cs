using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SharedObjectField : SharedVariableField<ObjectField, SharedObject, Object>
    {
        public SharedObjectField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
            this.fieldInfo = fieldInfo;
        }

        protected override ObjectField CreateEditorField()
        {
            ObjectField field = new ObjectField();
            field.objectType = typeof(Object);
            return field;
        }
    }

    public class SharedObjectResolver : FieldResolver<SharedObjectField, SharedObject>
    {
        public SharedObjectResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedObjectField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedObjectField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedObject);
        }
    }
}