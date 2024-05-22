namespace BehaviorDesigner
{
    public class ConditionalNode : TaskNode
    {
        protected override void CreatePorts()
        {
            CreateInputPort();
        }

        protected override void UpdateTaskIcon()
        {
            taskIconName = "Icons/ConditionalIcon";
            base.UpdateTaskIcon();
        }
    }
}