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
        private DropdownField behaviorDropdown;
        private List<IBehavior> behaviors;
        private List<string> choices;

        public void Init(BehaviorWindow window)
        {
            styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/BehaviorWindow"));
            saveAsBtn = this.Q<Button>("save-as-btn");
            leftBtn = this.Q<Button>("left-btn");
            rightBtn = this.Q<Button>("right-btn");
            behaviorDropdown = this.Q<DropdownField>("behavior-dp");
            behaviors = new List<IBehavior>();
            if (window.Behavior.Object is Component component)
            {
                component.GetComponents(behaviors);
            }
            else
            {
                behaviors.Add(window.Behavior);
            }

            choices = new List<string>(behaviors.Count);
            foreach (IBehavior behavior in behaviors)
            {
                choices.Add($"{behavior.Source.behaviorName} ({behavior.InstanceID})");
            }

            saveAsBtn.clicked += () =>
            {
                window.SaveAs();
            };

            leftBtn.clicked += () =>
            {
                int index = behaviorDropdown.index;
                if (--index < 0)
                {
                    index = choices.Count - 1;
                }

                if (index != behaviorDropdown.index)
                {
                    window.Init(behaviors[index]);
                }
            };
            
            rightBtn.clicked += () =>
            {
                int index = behaviorDropdown.index;
                if (++index >= choices.Count)
                {
                    index = 0;
                }

                if (index != behaviorDropdown.index)
                {
                    window.Init(behaviors[index]);
                }
            };

            behaviorDropdown.choices = choices;
            behaviorDropdown.SetValueWithoutNotify($"{window.Behavior.Source.behaviorName} ({window.Behavior.InstanceID})");
            behaviorDropdown.RegisterValueChangedCallback(evt =>
            {
                int index = choices.IndexOf(evt.newValue);
                window.Init(behaviors[index]);
            });
        }
    }
}