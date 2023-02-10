using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SharedColorField : SharedVariableField<ColorField, SharedColor, Color>
    {
        public SharedColorField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override ColorField CreateEditorField()
        {
            return new ColorField();
        }
    }

    public class SharedColorResolver : FieldResolver<SharedColorField, SharedColor>
    {
        public SharedColorResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedColorField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedColorField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedColor);
        }
    }
}