using System;
using System.Collections;
using System.Reflection;

namespace BehaviorDesigner.Editor
{
    public class SharedStringListField : SharedVariableField<BehaviorListField, SharedStringList, IList>
    {
        public SharedStringListField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
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

    public class SharedStringListResolver : FieldResolver<SharedStringListField, SharedStringList>
    {
        public SharedStringListResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedStringListField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedStringListField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedStringList);
        }
    }
}