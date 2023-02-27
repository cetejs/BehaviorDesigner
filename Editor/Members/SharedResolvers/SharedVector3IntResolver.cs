using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    public class SharedVector3IntField : SharedVariableField<Vector3IntField, SharedVector3Int, Vector3Int>
    {
        public SharedVector3IntField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override Vector3IntField CreateEditorField()
        {
            return new Vector3IntField();
        }
    }

    public class SharedVector3IntResolver : FieldResolver<SharedVector3IntField, SharedVector3Int>
    {
        public SharedVector3IntResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedVector3IntField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedVector3IntField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedVector3Int);
        }
    }
}