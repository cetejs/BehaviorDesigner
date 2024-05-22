using UnityEditor.Experimental.GraphView;

namespace BehaviorDesigner
{
    public class RootNode : ParentTaskNode
    {
        public override void Init(Task task, BehaviorWindow window)
        {
            base.Init(task, window);
            capabilities -= Capabilities.Deletable;
        }

        protected override void CreatePorts()
        {
            CreateOutputPort();
        }

        protected override void UpdateTaskIcon()
        {
            taskIconName = "Icons/RootIcon";
            base.UpdateTaskIcon();
        }
    }
}