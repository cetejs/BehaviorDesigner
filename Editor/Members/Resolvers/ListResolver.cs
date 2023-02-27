using System;
using System.Collections;
using System.Reflection;

namespace BehaviorDesigner.Editor
{
    public class ListResolver : FieldResolver<ObjectListField, IList>
    {
        public ListResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override ObjectListField CreateEditorField(FieldInfo fieldInfo)
        {
            Type genericType = fieldInfo.FieldType.GetGenericArguments()[0];
            return new ObjectListField(window, genericType, fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return typeof(IList).IsAssignableFrom(info.FieldType) && !info.FieldType.IsArray;
        }
    }
}