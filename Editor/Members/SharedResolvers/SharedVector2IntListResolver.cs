using System;
using System.Collections;
using System.Reflection;

namespace BehaviorDesigner.Editor
{
    public class SharedVector2IntListField : SharedVariableField<ObjectListField, SharedVector2IntList, IList>
    {
        public SharedVector2IntListField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
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

    public class SharedVector2IntListResolver : FieldResolver<SharedVector2IntListField, SharedVector2IntList>
    {
        public SharedVector2IntListResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedVector2IntListField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedVector2IntListField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedVector2IntList);
        }
    }
}