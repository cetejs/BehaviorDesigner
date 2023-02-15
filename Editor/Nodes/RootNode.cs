using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class RootNode : ParentTaskNode
    {
        public override void Init(Task task, BehaviorWindow window)
        {
            this.task = task;
            this.window = window;
            parentTask = task as ParentTask;
            capabilities -= Capabilities.Deletable;
            SetPosition(task.graphPosition);
            AddChild();
            Restore();
            AddDoubleClickSelection();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            AddScriptMenuItem(evt);
        }
    }
}