using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class BehaviorNameView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<BehaviorNameView, UxmlTraits> {}
        
        private Label nameLabel;
        
        public void Init(BehaviorWindow window)
        {
            styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/BehaviorWindow"));
            nameLabel = this.Q<Label>();
            nameLabel.text = $"{window.Behavior.Object.name} - {window.Behavior.Source.behaviorName} ({window.Behavior.InstanceID})";
        }
    }
}