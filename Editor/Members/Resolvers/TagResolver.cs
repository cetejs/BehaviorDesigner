using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class TagResolver : FieldResolver<TagField, string>
    {
        public TagResolver(FieldInfo fieldInfo, BehaviorWindow window) : base(fieldInfo, window)
        {
        }
        
        public override void Register(SharedVariable variable)
        {
            editorField.RegisterValueChangedCallback(evt =>
            {
                window.RegisterUndo("Update Variable");
                variable.SetValue((Tag) evt.newValue);
                window.Save();
            });
        }

        public override void Restore(SharedVariable variable)
        {
            editorField.value = (Tag) variable.GetValue();
        }

        public override void Restore(Task task)
        {
            editorField.value = (Tag)fieldInfo.GetValue(task);
        }

        public override void Save(Task task)
        {
            fieldInfo.SetValue(task, (Tag) editorField.value);
        }

        protected override TagField CreateEditorField(FieldInfo fieldInfo)
        {
            return new TagField(fieldInfo.Name);
        }

        public static bool IsAcceptable(FieldInfo info)
        {
            return info.FieldType == typeof(Tag);
        }
    }
}