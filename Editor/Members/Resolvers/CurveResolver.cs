using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

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

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(AnimationCurve);
        }
    }
}