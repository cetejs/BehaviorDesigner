using System.Reflection;
using UnityEngine.UIElements;

namespace BehaviorDesigner
{
    public class DescriptionView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<DescriptionView, UxmlTraits> {}

        private BehaviorWindow window;
        private Label descriptionLabel;

        public void Init(BehaviorWindow window)
        {
            this.window = window;
            descriptionLabel = this.Q<Label>();
        }

        public void Refresh()
        {
            OnNodeSelected(null);
        }

        public void OnNodeSelected(TaskNode node)
        {
            TaskDescriptionAttribute attribute;
            if (node != null && (attribute = node.Task.GetType().GetCustomAttribute<TaskDescriptionAttribute>()) != null)
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

        public void OnNodeUnselected(TaskNode node)
        {
            descriptionLabel.text = null;
            descriptionLabel.visible = false;
        }
    }
}