using System;
using System.Reflection;
using UnityEditor.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SharedEnumField : SharedVariableField<EnumField, SharedVariable, Enum>
    {
        public SharedEnumField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }
    
        protected override EnumField CreateEditorField()
        {
            Array array = Enum.GetValues(fieldInfo.FieldType.BaseType.GetGenericArguments()[0]);
            return new EnumField(fieldInfo.Name, (Enum) array.GetValue(0));
        }

        protected override void DrawValue(object obj)
        {
            editorField.value = (Enum)obj;
        }

        protected override void SaveValue(Enum obj)
        {
            value.SetValue(obj);
        }
    }
    
    public class SharedEnumResolver : FieldResolver<SharedEnumField, SharedVariable>
    {
        public SharedEnumResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }
    
        protected override SharedEnumField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedEnumField(fieldInfo, window);
        }
    
        public static bool IsAcceptable(FieldInfo info)
        {
            if (!info.FieldType.IsSubclassOf(typeof(SharedVariable)))
            {
                return false;
            }
            
            return info.FieldType.BaseType.GetGenericArguments()[0].IsEnum;
        }
    }
}