using System;
using System.Collections;
using System.Reflection;

namespace BehaviorDesigner.Editor
{
    public class SharedColorListField : SharedVariableField<BehaviorListField, SharedColorList, IList>
    {
        public SharedColorListField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
            this.fieldInfo = fieldInfo;
        }

        protected override BehaviorListField CreateEditorField()
        {
            Type genericType = fieldInfo.FieldType.BaseType.GetGenericArguments()[0];
            return new BehaviorListField(window, genericType, fieldInfo.Name);
        }

        protected override void SetFieldName(string name)
        {
        }
    }

    public class SharedColorListResolver : FieldResolver<SharedColorListField, SharedColorList>
    {
        public SharedColorListResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedColorListField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedColorListField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedColorList);
        }
    }
}