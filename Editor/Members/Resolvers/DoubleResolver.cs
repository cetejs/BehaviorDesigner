using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class DoubleResolver : FieldResolver<DoubleField, double>
    {
        public DoubleResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override DoubleField CreateEditorField(FieldInfo fieldInfo)
        {
            return new DoubleField(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(double);
        }
    }
}