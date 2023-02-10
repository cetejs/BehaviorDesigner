using System;
using System.Collections;
using System.Reflection;

namespace BehaviorDesigner.Editor
{
    public class SharedVector4ListField : SharedVariableField<BehaviorListField, SharedVector4List, IList>
    {
        public SharedVector4ListField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
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

    public class SharedVector4ListResolver : FieldResolver<SharedVector4ListField, SharedVector4List>
    {
        public SharedVector4ListResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedVector4ListField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedVector4ListField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedVector4List);
        }
    }
}