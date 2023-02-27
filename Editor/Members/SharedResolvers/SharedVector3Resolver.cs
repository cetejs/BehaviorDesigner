using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    public class SharedVector3Field : SharedVariableField<Vector3Field, SharedVector3, Vector3>
    {
        public SharedVector3Field(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override Vector3Field CreateEditorField()
        {
            return new Vector3Field();
        }
    }

    public class SharedVector3Resolver : FieldResolver<SharedVector3Field, SharedVector3>
    {
        public SharedVector3Resolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedVector3Field CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedVector3Field(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedVector3);
        }
    }
}