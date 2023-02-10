using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SharedVector2IntField : SharedVariableField<Vector2IntField, SharedVector2Int, Vector2Int>
    {
        public SharedVector2IntField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override Vector2IntField CreateEditorField()
        {
            return new Vector2IntField();
        }
    }

    public class SharedVector2IntResolver : FieldResolver<SharedVector2IntField, SharedVector2Int>
    {
        public SharedVector2IntResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedVector2IntField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedVector2IntField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedVector2Int);
        }
    }
}