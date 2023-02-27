using System.Reflection;
using UnityEditor.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SharedTagField : SharedVariableField<TagField, SharedTag, string>
    {
        public SharedTagField(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override TagField CreateEditorField()
        {
            return new TagField();
        }

        protected override void DrawValue(object obj)
        {
            editorField.value = (Tag)obj;
        }

        protected override void SaveValue(string obj)
        {
            value.Value = obj;
        }
    }

    public class SharedTagResolver : FieldResolver<SharedTagField, SharedTag>
    {
        public SharedTagResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }

        protected override SharedTagField CreateEditorField(FieldInfo fieldInfo)
        {
            return new SharedTagField(fieldInfo, window);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(SharedTag);
        }
    }
}