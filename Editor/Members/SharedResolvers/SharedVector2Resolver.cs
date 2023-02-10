using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SharedVector2Field : SharedVariableField<Vector2Field, SharedVector2, Vector2>
    {
        public SharedVector2Field(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override Vector2Field CreateEditorField()
        {
            return new Vector2Field();
        }
    }

    public class SharedVector2Resolver : FieldResolver<SharedVector2Field, SharedVector2>
    {
        public SharedVector2Resolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedVector2Field CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedVector2Field(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedVector2);
        }
    }
}