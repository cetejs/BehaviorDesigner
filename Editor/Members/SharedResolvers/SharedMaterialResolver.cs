using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SharedMaterialField : SharedVariableField<ObjectField, SharedMaterial, Object>
    {
        public SharedMaterialField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
            this.fieldInfo = fieldInfo;
        }

        protected override ObjectField CreateEditorField()
        {
            ObjectField field = new ObjectField();
            field.objectType = typeof(Material);
            return field;
        }
    }

    public class SharedMaterialResolver : FieldResolver<SharedMaterialField, SharedMaterial>
    {
        public SharedMaterialResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedMaterialField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedMaterialField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedMaterial);
        }
    }
}