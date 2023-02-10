using System;
using System.Collections;
using System.Reflection;
using UnityEditor;

namespace BehaviorDesigner.Editor
{
    public class ListResolver : FieldResolver<BehaviorListField, IList>
    {
        public ListResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override BehaviorListField CreateEditorField(FieldInfo fieldInfo)
        {
            Type genericType = fieldInfo.FieldType.GetGenericArguments()[0];
            return new BehaviorListField(window, genericType, fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return typeof(IList).IsAssignableFrom(info.FieldType) && !info.FieldType.IsArray;
        }
    }
}