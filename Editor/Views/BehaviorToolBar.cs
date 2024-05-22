using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner
{
    public class BehaviorToolBar : Toolbar
    {
        public new class UxmlFactory : UxmlFactory<BehaviorToolBar, UxmlTraits> { }

        private BehaviorWindow window;
        private Button leftBtn;
        private Button rightBtn;
        private Button exportBtn;
        private DropdownField behaviorDp;
        private int lastIndex;
        private int selectedIndex;
        private bool isUndoSelected;
        private List<IBehavior> behaviors = new List<IBehavior>();
        private List<string> choices = new List<string>();
        private List<int> selectedIndexes = new List<int>();

        public void Init(BehaviorWindow window)
        {
            this.window = window;
            leftBtn = this.Q<Button>("left-btn");
            rightBtn = this.Q<Button>("right-btn");
            exportBtn = this.Q<Button>("export-btn");
            behaviorDp = this.Q<DropdownField>("behavior-dp");
            Refresh();

            leftBtn.clicked += () =>
            {
                Select(selectedIndex - 1);
            };

            rightBtn.clicked += () =>
            {
                Select(selectedIndex + 1);
            };

            exportBtn.clicked += () =>
            {
                window.ExportBehavior();
            };

            behaviorDp.RegisterValueChangedCallback(evt =>
            {
                int index = choices.IndexOf(evt.newValue);
                IBehavior behavior = behaviors[index];
                Selection.activeObject = behavior.Object;
                window.SetBehavior(behavior);
            });
        }

        private void Select(int index)
        {
            index = Mathf.Clamp(index, 0, selectedIndexes.Count - 1);
            if (selectedIndex == index)
            {
                return;
            }

            selectedIndex = index;
            isUndoSelected = true;
            IBehavior behavior = behaviors[selectedIndexes[index]];
            Selection.activeObject = behavior.Object;
            window.SetBehavior(behavior);
            isUndoSelected = false;
        }

        public void Refresh()
        {
            behaviors.Clear();
            choices.Clear();
            behaviors.AddRange(Resources.FindObjectsOfTypeAll<BehaviorTree>());
            behaviors.AddRange(Resources.FindObjectsOfTypeAll<ExternalBehavior>());
            foreach (IBehavior behavior in behaviors)
            {
                string choices = $"{behavior.Object.name} - {behavior.Source.behaviorName}";
                this.choices.Add(choices);
            }

            behaviorDp.choices = choices;
            int index = behaviors.IndexOf(window.Behavior);
            behaviorDp.SetValueWithoutNotify(index >= 0 ? choices[index] : "{None Selected}");
            if (!isUndoSelected && index >= 0 && lastIndex != index)
            {
                if (selectedIndex < selectedIndexes.Count)
                {
                    selectedIndexes.RemoveRange(selectedIndex + 1, selectedIndexes.Count - selectedIndex - 1);
                }

                selectedIndexes.Add(index);
                lastIndex = index;
                selectedIndex = selectedIndexes.Count - 1;
            }

            leftBtn.SetEnabled(selectedIndex > 0);
            rightBtn.SetEnabled(selectedIndex < selectedIndexes.Count - 1);
        }

        public void ClearSelection()
        {
            lastIndex = -1;
            selectedIndex = -1;
            selectedIndexes.Clear();
            leftBtn.SetEnabled(false);
            rightBtn.SetEnabled(false);
        }
    }
}