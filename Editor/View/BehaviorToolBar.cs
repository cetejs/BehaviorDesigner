using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class BehaviorToolBar : Toolbar
    {
        public new class UxmlFactory : UxmlFactory<BehaviorToolBar, UxmlTraits> { }
        
        private Button saveAsBtn;
        private Button leftBtn;
        private Button rightBtn;
        private DropdownField behaviorDp;
        private List<IBehavior> behaviors;
        private List<string> choices;

        public void Init(BehaviorWindow window)
        {
            styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/BehaviorWindow"));
            saveAsBtn = this.Q<Button>("save-as-btn");
            leftBtn = this.Q<Button>("left-btn");
            rightBtn = this.Q<Button>("right-btn");
            behaviorDp = this.Q<DropdownField>("behavior-dp");
            behaviors = new List<IBehavior>();
            behaviors.AddRange(Resources.FindObjectsOfTypeAll<Behavior>());
            behaviors.AddRange(Resources.FindObjectsOfTypeAll<ExternalBehavior>());
            choices = new List<string>(behaviors.Count);
            foreach (IBehavior behavior in behaviors)
            {
                string choices = $"{behavior.Object.name} - {behavior.Source.behaviorName}";
                if (behavior is ExternalBehavior)
                {
                    choices += " (external)";
                }
                
                this.choices.Add(choices);
            }
            

            saveAsBtn.clicked += () =>
            {
                window.SaveAs();
            };

            leftBtn.clicked += () =>
            {
                window.UndoSelect(false);
            };
            
            rightBtn.clicked += () =>
            {
                window.UndoSelect(true);
            };

            behaviorDp.choices = choices;
            int index = behaviors.IndexOf(window.Behavior);
            behaviorDp.SetValueWithoutNotify(index >= 0 ?choices[index] : "{None Selected}");
            behaviorDp.RegisterValueChangedCallback(evt =>
            {
                index = choices.IndexOf(evt.newValue);
                window.Init(behaviors[index]);
            });
        }
    }
}