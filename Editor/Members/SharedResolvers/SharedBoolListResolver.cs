using System;
using System.Collections;
using System.Reflection;

namespace BehaviorDesigner.Editor
{
    public class SharedBoolListField : SharedVariableField<BehaviorListField, SharedBoolList, IList>
    {
        public SharedBoolListField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
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

    public class SharedBoolListResolver : FieldResolver<SharedBoolListField, SharedBoolList>
    {
        public SharedBoolListResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedBoolListField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedBoolListField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedBoolList);
        }
    }
}