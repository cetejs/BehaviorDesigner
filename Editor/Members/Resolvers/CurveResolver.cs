using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    public class CurveResolver : FieldResolver<CurveField, AnimationCurve>
    {
        public CurveResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override CurveField CreateEditorField(FieldInfo fieldInfo)
        {
            return new CurveField(fieldInfo.Name);
        }

        public override void Restore(Task task)
        {
            object value = fieldInfo.GetValue(task);
            if (value == null)
            {
                value = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
            }

            editorField.value = (AnimationCurve) value;
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(AnimationCurve);
        }
    }
}