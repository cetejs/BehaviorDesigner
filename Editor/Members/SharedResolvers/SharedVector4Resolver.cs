using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SharedVector4Field : SharedVariableField<Vector4Field, SharedVector4, Vector4>
    {
        public SharedVector4Field(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override Vector4Field CreateEditorField()
        {
            return new Vector4Field();
        }
    }

    public class SharedVector4Resolver : FieldResolver<SharedVector4Field, SharedVector4>
    {
        public SharedVector4Resolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedVector4Field CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedVector4Field(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedVector4);
        }
    }
}