using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class BoundsIntResolver : FieldResolver<BoundsIntField, BoundsInt>
    {
        public BoundsIntResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override BoundsIntField CreateEditorField(FieldInfo fieldInfo)
        {
            return new BoundsIntField(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(BoundsInt);
        }
    }
}