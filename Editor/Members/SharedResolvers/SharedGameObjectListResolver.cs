using System;
using System.Collections;
using System.Reflection;

namespace BehaviorDesigner.Editor
{
    public class SharedGameObjectListField : SharedVariableField<ObjectListField, SharedGameObjectList, IList>
    {
        public SharedGameObjectListField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
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

    public class SharedGameObjectListResolver : FieldResolver<SharedGameObjectListField, SharedGameObjectList>
    {
        public SharedGameObjectListResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedGameObjectListField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedGameObjectListField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedGameObjectList);
        }
    }
}