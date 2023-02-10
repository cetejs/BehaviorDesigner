using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class Vector2IntResolver : FieldResolver<Vector2IntField, Vector2Int>
    {
        public Vector2IntResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override Vector2IntField CreateEditorField(FieldInfo fieldInfo)
        {
            return new Vector2IntField(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(Vector2Int);
        }
    }
}