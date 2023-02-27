using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    public class RectResolver : FieldResolver<RectField, Rect>
    {
        public RectResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override RectField CreateEditorField(FieldInfo fieldInfo)
        {
            return new RectField(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(Rect);
        }
    }
}