using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class QuaternionResolver : FieldResolver<Vector4Field, Vector4>
    {
        public QuaternionResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }
        
        public override void Register(SharedVariable variable)
        {
            editorField.RegisterValueChangedCallback(evt =>
            {
                window.RegisterUndo("Update Variable");
                variable.SetValue(ToQuaternion(evt.newValue));
                window.Save();
            });
        }

        public override void Restore(SharedVariable variable)
        { 
            editorField.value = ToVector4((Quaternion)variable.GetValue());
        }

        public override void Restore(Task task)
        {
            editorField.value = ToVector4((Quaternion)fieldInfo.GetValue(task));
        }

        public override void Save(Task task)
        {
            fieldInfo.SetValue(task, ToQuaternion(editorField.value));
        }

        protected override Vector4Field CreateEditorField(FieldInfo fieldInfo)
        {
            return new Vector4Field(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(Quaternion);
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
}