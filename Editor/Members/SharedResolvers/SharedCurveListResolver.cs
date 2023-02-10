using System;
using System.Collections;
using System.Reflection;

namespace BehaviorDesigner.Editor
{
    public class SharedCurveListField : SharedVariableField<BehaviorListField, SharedAnimationCurveList, IList>
    {
        public SharedCurveListField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
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

    public class SharedCurveListResolver : FieldResolver<SharedCurveListField, SharedAnimationCurveList>
    {
        public SharedCurveListResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedCurveListField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedCurveListField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedAnimationCurveList);
        }
    }
}