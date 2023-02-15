using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class BehaviorNameView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<BehaviorNameView, UxmlTraits> {}
        
        private Label nameLabel;

        public void Init()
        {
            styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/BehaviorWindow"));
            nameLabel = this.Q<Label>();
            if (Selection.activeObject is GameObject)
            {
                nameLabel.text = "Right Click, Add a Behavior Tree Component";
            }
            else
            {
                nameLabel.text = "Select a GameObject";
            }
        }

        public void Init(BehaviorWindow window)
        {
            styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/BehaviorWindow"));
            nameLabel = this.Q<Label>();
            nameLabel.text = $"{window.Behavior.Object.name} - {window.Behavior.Source.behaviorName} ({window.Behavior.InstanceID})";
        }
    }
}