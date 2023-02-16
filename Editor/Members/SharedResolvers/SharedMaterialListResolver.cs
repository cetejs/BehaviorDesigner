using System;
using System.Collections;
using System.Reflection;

namespace BehaviorDesigner.Editor
{
    public class SharedMaterialListField : SharedVariableField<ObjectListField, SharedMaterialList, IList>
    {
        public SharedMaterialListField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
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

    public class SharedMaterialListResolver : FieldResolver<SharedMaterialListField, SharedMaterialList>
    {
        public SharedMaterialListResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedMaterialListField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedMaterialListField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedMaterialList);
        }
    }
}