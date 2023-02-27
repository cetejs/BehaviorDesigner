using System;
using System.Collections;
using System.Reflection;

namespace BehaviorDesigner.Editor
{
    public class SharedListField : SharedVariableField<ObjectListField, SharedVariable, IList>
    {
        public SharedListField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
            this.fieldInfo = fieldInfo;
        }

        protected override ObjectListField CreateEditorField()
        {
            Type genericType = fieldInfo.FieldType.BaseType.GetGenericArguments()[0];
            return new ObjectListField(window, genericType, fieldInfo.Name);
        }

        protected override void SetFieldName(string name)
        {
        }
    }

    public class SharedObjectListResolver : FieldResolver<SharedListField, SharedVariable>
    {
        public SharedObjectListResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedListField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedListField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            if (!info.FieldType.IsSubclassOf(typeof(SharedVariable)))
            {
                return false;
            }

            if (info.FieldType.BaseType.GetGenericTypeDefinition() != typeof(SharedList<>))
            {
                return false;
            }

            return typeof(Object).IsAssignableFrom(info.FieldType.BaseType.GetGenericArguments()[0]);
        }
    }
}