using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    public class SharedBehaviourField : SharedVariableField<ObjectField, SharedBehaviour, Object>
    {
        public SharedBehaviourField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
            this.fieldInfo = fieldInfo;
        }

        protected override ObjectField CreateEditorField()
        {
            ObjectField field = new ObjectField();
            field.objectType = typeof(Behaviour);
            return field;
        }
    }

    public class SharedBehaviourResolver : FieldResolver<SharedBehaviourField, SharedBehaviour>
    {
        public SharedBehaviourResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedBehaviourField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedBehaviourField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedBehaviour);
        }
    }
}