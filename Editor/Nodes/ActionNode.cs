namespace BehaviorDesigner
{
    public class ActionNode : TaskNode
    {
        protected override void CreatePorts()
        {
            CreateInputPort();
        }

        protected override void UpdateTaskIcon()
        {
            taskIconName = "Icons/ActionIcon";
            base.UpdateTaskIcon();
        }
    }
}