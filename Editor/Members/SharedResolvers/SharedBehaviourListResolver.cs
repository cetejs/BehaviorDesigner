using System;
using System.Collections;
using System.Reflection;

namespace BehaviorDesigner.Editor
{
    public class SharedBehaviourListField : SharedVariableField<BehaviorListField, SharedBehaviourList, IList>
    {
        public SharedBehaviourListField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
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

    public class SharedBehaviourListResolver : FieldResolver<SharedBehaviourListField, SharedBehaviourList>
    {
        public SharedBehaviourListResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedBehaviourListField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedBehaviourListField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedBehaviourList);
        }
    }
}