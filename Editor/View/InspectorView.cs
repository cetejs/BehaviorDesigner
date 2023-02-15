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
        private TaskNode taskNode;
        private Task task;

        public void Init(BehaviorWindow window)
        {
            this.window = window;
            styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/InspectorView"));
            scrollView = this.Q<ScrollView>();
            toolbarMenu = parent.Q<ToolbarMenu>();
            toolbarMenu.menu.AppendAction("Edit Script", action =>
            {
                if (task != null)
                {
                    BehaviorUtils.OpenScript(task);
                }
            });
            
            toolbarMenu.menu.AppendAction("Locate Script", action =>
            {
                if (task != null)
                {
                    BehaviorUtils.SelectScript(task);
                }
            });
            
            toolbarMenu.menu.AppendAction("Reset", action =>
            {
                if (taskNode != null)
                {
                    taskNode.Reset();
                    Restore();
                }
            });
            
            toolbarMenu.SetEnabled(false);
        }

        public void DoDraw()
        {
            if (window.View.selection.Count == 0)
            {
                if (task != null)
                {
                    task = null;
                    taskNode = null;
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
                        taskNode = node;
                        taskNode.OnGUI(scrollView);
                        toolbarMenu.SetEnabled(true);
                    }
                    
                    break;
                }
            }
        }

        public void Restore()
        {
            if (taskNode != null)
            {
                taskNode.OnGUI(scrollView);
                toolbarMenu.SetEnabled(true);
            }
            else
            {
                scrollView.Clear();
                toolbarMenu.SetEnabled(false);
            }
        }
    }
}