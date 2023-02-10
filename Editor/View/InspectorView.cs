using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> {}

        private BehaviorWindow window;
        private ScrollView scrollView;
        private ToolbarMenu toolbarMenu;
        private IInspectorGUI inspector;

        public void Init(BehaviorWindow window)
        {
            this.window = window;
            styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/InspectorView"));
            scrollView = this.Q<ScrollView>();
            toolbarMenu = parent.Q<ToolbarMenu>();
            toolbarMenu.menu.AppendAction("Edit Script", action =>
            {
                if (inspector != null)
                {
                    BehaviorUtils.OpenScript(inspector.Task);
                }
            });
            
            toolbarMenu.menu.AppendAction("Locate Script", action =>
            {
                if (inspector != null)
                {
                    BehaviorUtils.SelectScript(inspector.Task);
                }
            });
            
            toolbarMenu.menu.AppendAction("Reset", action =>
            {
                if (inspector != null)
                {
                    inspector.Reset();
                    Restore();
                }
            });
            
            toolbarMenu.SetEnabled(false);
        }

        public void DoDraw()
        {
            for (int i = window.View.selection.Count - 1; i >= 0; i--)
            {
                if (window.View.selection[i] is IInspectorGUI selectable)
                {
                    if (inspector != selectable)
                    {
                        inspector = selectable;
                        inspector.OnGUI(scrollView);
                        toolbarMenu.SetEnabled(true);
                    }
                    
                    break;
                }
            }
        }

        public void Restore()
        {
            if (inspector != null)
            {
                TaskNode node = FindNode(inspector.Task);
                if (node != null)
                {
                    node.OnGUI(scrollView);
                }
            }
        }

        private TaskNode FindNode(Task task)
        {
            foreach (VisualElement child in window.View.graphElements)
            {
                if (child is TaskNode node && node.Task.guid == task.guid)
                {
                    if (node.Task.guid == task.guid)
                    {
                        return node;
                    }
                }
            }
            
            return null;
        }
    }
}