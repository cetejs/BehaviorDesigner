using System;
using System.Collections;
using System.Reflection;

namespace BehaviorDesigner.Editor
{
    public class SharedVector3IntListField : SharedVariableField<ObjectListField, SharedVector3IntList, IList>
    {
        public SharedVector3IntListField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
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

    public class SharedVector3IntListResolver : FieldResolver<SharedVector3IntListField, SharedVector3IntList>
    {
        public SharedVector3IntListResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedVector3IntListField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedVector3IntListField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedVector3IntList);
        }
    }
}