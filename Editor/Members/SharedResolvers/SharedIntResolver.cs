using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SharedIntField : SharedVariableField<IntegerField, SharedInt, int>
    {
        public SharedIntField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override IntegerField CreateEditorField()
        {
            return new IntegerField();
        }
    }

    public class SharedIntResolver : FieldResolver<SharedIntField, SharedInt>
    {
        public SharedIntResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedIntField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedIntField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedInt);
        }
    }
}