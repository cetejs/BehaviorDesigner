using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    public class Vector3Resolver : FieldResolver<Vector3Field, Vector3>
    {
        public Vector3Resolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override Vector3Field CreateEditorField(FieldInfo fieldInfo)
        {
            return new Vector3Field(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(Vector3);
        }
    }
}