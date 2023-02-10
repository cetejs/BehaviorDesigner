using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class ObjectResolver : FieldResolver<ObjectField, Object>
    {
        public ObjectResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override ObjectField CreateEditorField(FieldInfo fieldInfo)
        {
            ObjectField field = new ObjectField(fieldInfo.Name);
            field.objectType = fieldInfo.FieldType;
            return field;
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(Object);
        }
    }
}