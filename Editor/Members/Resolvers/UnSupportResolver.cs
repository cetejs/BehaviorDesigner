using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class UnSupportResolver : IFieldResolver
    {
        public VisualElement EditorField { get; }

        public UnSupportResolver(FieldInfo fieldInfo)
        {
            EditorField = new TextField()
            {
                label = ObjectNames.NicifyVariableName(fieldInfo.Name),
                value = $"Unsupported Type: {fieldInfo.FieldType}"
            };
            
            EditorField.SetEnabled(false);
        }

        public void Register()
        {
        }

        public void Register(SharedVariable variable)
        {
        }

        public void Restore(SharedVariable variable)
        {
        }

        public void Restore(Task task)
        {
        }

        public void Save(Task task)
        {
        }
    }
}