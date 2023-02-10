using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    public class SharedGameObjectField : SharedVariableField<ObjectField, SharedGameObject, Object>
    {
        public SharedGameObjectField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
            this.fieldInfo = fieldInfo;
        }

        protected override ObjectField CreateEditorField()
        {
            ObjectField field = new ObjectField();
            field.objectType = typeof(GameObject);
            return field;
        }
    }

    public class SharedGameObjectResolver : FieldResolver<SharedGameObjectField, SharedGameObject>
    {
        public SharedGameObjectResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedGameObjectField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedGameObjectField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedGameObject);
        }
    }
}