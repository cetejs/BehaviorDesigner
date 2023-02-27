using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    public class SharedQuaternionField : SharedVariableField<Vector4Field, SharedQuaternion, Vector4>
    {
        public SharedQuaternionField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override Vector4Field CreateEditorField()
        {
            return new Vector4Field();
        }

        protected override void DrawValue(object obj)
        {
            editorField.value = ToVector4((Quaternion) obj);
        }

        protected override void SaveValue(Vector4 obj)
        {
            value.Value = ToQuaternion(obj);
        }

        private Quaternion ToQuaternion(Vector4 v4)
        {
            return new Quaternion(v4.x, v4.y, v4.z, v4.w);
        }

        private Vector4 ToVector4(Quaternion q)
        {
            return new Vector4(q.x, q.y, q.z, q.w);
        }
    }

    public class SharedQuaternionResolver : FieldResolver<SharedQuaternionField, SharedQuaternion>
    {
        public SharedQuaternionResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedQuaternionField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedQuaternionField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedQuaternion);
        }
    }
}