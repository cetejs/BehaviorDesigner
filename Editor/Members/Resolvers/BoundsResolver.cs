using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    public class BoundsResolver : FieldResolver<BoundsField, Bounds>
    {
        public BoundsResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override BoundsField CreateEditorField(FieldInfo fieldInfo)
        {
            return new BoundsField(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(Bounds);
        }
    }
}