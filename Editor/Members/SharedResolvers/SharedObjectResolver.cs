using System;
using System.Reflection;
using UnityEditor.UIElements;
using Object = UnityEngine.Object;

namespace BehaviorDesigner.Editor
{
    public class SharedObjectField : SharedVariableField<ObjectField, SharedVariable, Object>
    {
        public SharedObjectField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
            this.fieldInfo = fieldInfo;
        }

        protected override ObjectField CreateEditorField()
        {
            ObjectField field = new ObjectField();
            field.objectType = fieldInfo.FieldType.BaseType.GetGenericArguments()[0];
            return field;
        }
    }

    public class SharedObjectResolver : FieldResolver<SharedObjectField, SharedVariable>
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
            if (!info.FieldType.IsSubclassOf(typeof(SharedVariable)))
            {
                return false;
            }

            if (info.FieldType.BaseType.GetGenericTypeDefinition() != typeof(SharedVariable<>))
            {
                return false;
            }

            return typeof(Object).IsAssignableFrom(info.FieldType.BaseType.GetGenericArguments()[0]);
        }
    }
}