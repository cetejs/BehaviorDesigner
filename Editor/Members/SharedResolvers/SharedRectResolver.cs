using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    public class SharedRectField : SharedVariableField<RectField, SharedRect, Rect>
    {
        public SharedRectField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override RectField CreateEditorField()
        {
            return new RectField();
        }
    }

    public class SharedRectResolver : FieldResolver<SharedRectField, SharedRect>
    {
        public SharedRectResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedRectField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedRectField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedRect);
        }
    }
}