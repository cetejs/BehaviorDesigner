using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    public class SharedCurveField : SharedVariableField<CurveField, SharedAnimationCurve, AnimationCurve>
    {
        public SharedCurveField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override CurveField CreateEditorField()
        {
            return new CurveField();
        }
    }

    public class SharedCurveResolver : FieldResolver<SharedCurveField, SharedAnimationCurve>
    {
        public SharedCurveResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedCurveField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedCurveField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedAnimationCurve);
        }
    }
}