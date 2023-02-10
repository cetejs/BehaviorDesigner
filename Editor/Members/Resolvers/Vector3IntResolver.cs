using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class Vector3IntResolver : FieldResolver<Vector3IntField, Vector3Int>
    {
        public Vector3IntResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override Vector3IntField CreateEditorField(FieldInfo fieldInfo)
        {
            return new Vector3IntField(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(Vector3Int);
        }
    }
}