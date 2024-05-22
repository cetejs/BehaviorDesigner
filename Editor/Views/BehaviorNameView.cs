using UnityEngine.UIElements;

namespace BehaviorDesigner
{
    public class BehaviorNameView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<BehaviorNameView, UxmlTraits> {}

        private BehaviorWindow window;
        private Label nameLabel;

        public void Init(BehaviorWindow window)
        {
            nameLabel = this.Q<Label>();
            this.window = window;
            Refresh();
        }

        public void Refresh()
        {
            if (window.Source == null)
            {
                nameLabel.text = "Right Click, Add a Behavior Tree Component";
            }
            else
            {
                nameLabel.text = $"{window.Behavior.Object.name} - {window.Source.behaviorName} ({window.Behavior.Object.GetInstanceID()})";
            }
        }
    }
}