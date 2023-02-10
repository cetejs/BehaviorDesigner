using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class Vector2Resolver : FieldResolver<Vector2Field, Vector2>
    {
        public Vector2Resolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override Vector2Field CreateEditorField(FieldInfo fieldInfo)
        {
            return new Vector2Field(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(Vector2);
        }
    }
}