using System.Reflection;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class DescriptionView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<DescriptionView, UxmlTraits> {}
        
        private Label descriptionLabel;
        private BehaviorWindow window;
        private Task task;

        public void Init(BehaviorWindow window)
        {
            this.window = window;
            styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/BehaviorWindow"));
            descriptionLabel = this.Q<Label>();
        }

        public void DoDraw()
        {
            if (window.View.selection.Count == 0)
            {
                if (task != null)
                {
                    task = null;
                    Restore();
                }

                return;
            }

            for (int i = window.View.selection.Count - 1; i >= 0; i--)
            {
                if (window.View.selection[i] is TaskNode node)
                {
                    if (task != node.Task)
                    {
                        task = node.Task;
                        Restore();
                    }
                }
            }
        }

        public void Restore()
        {
            TaskDescriptionAttribute attribute;
            if (task != null && (attribute = task.GetType().GetCustomAttribute<TaskDescriptionAttribute>()) != null)
            {
                descriptionLabel.text = attribute.description;
                descriptionLabel.visible = true;
            }
            else
            {
                descriptionLabel.text = null;
                descriptionLabel.visible = false;
            }
        }
    }
}