using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SharedDoubleField : SharedVariableField<DoubleField, SharedDouble, double>
    {
        public SharedDoubleField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override DoubleField CreateEditorField()
        {
            return new DoubleField();
        }
    }

    public class SharedDoubleResolver : FieldResolver<SharedDoubleField, SharedDouble>
    {
        public SharedDoubleResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedDoubleField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedDoubleField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedDouble);
        }
    }
}