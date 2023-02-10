using System;
using System.Collections;
using System.Reflection;

namespace BehaviorDesigner.Editor
{
    public class SharedDoubleListField : SharedVariableField<BehaviorListField, SharedDoubleList, IList>
    {
        public SharedDoubleListField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
            this.fieldInfo = fieldInfo;
        }

        protected override BehaviorListField CreateEditorField()
        {
            Type genericType = fieldInfo.FieldType.BaseType.GetGenericArguments()[0];
            return new BehaviorListField(window, genericType, fieldInfo.Name);
        }

        protected override void SetFieldName(string name)
        {
        }
    }

    public class SharedDoubleListResolver : FieldResolver<SharedDoubleListField, SharedDoubleList>
    {
        public SharedDoubleListResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedDoubleListField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedDoubleListField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedDoubleList);
        }
    }
}