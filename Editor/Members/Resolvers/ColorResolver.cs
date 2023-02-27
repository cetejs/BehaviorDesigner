using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    public class ColorResolver : FieldResolver<ColorField, Color>
    {
        public ColorResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override ColorField CreateEditorField(FieldInfo fieldInfo)
        {
            return new ColorField(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(Color);
        }
    }
}