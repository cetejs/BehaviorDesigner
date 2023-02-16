using System;
using System.Collections;
using System.Reflection;

namespace BehaviorDesigner.Editor
{
    public class SharedQuaternionListField : SharedVariableField<ObjectListField, SharedQuaternionList, IList>
    {
        public SharedQuaternionListField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
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

    public class SharedQuaternionListResolver : FieldResolver<SharedQuaternionListField, SharedQuaternionList>
    {
        public SharedQuaternionListResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedQuaternionListField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedQuaternionListField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedQuaternionList);
        }
    }
}