namespace BehaviorDesigner
{
    public class DecoratorNode : ParentTaskNode
    {
        protected override void CreatePorts()
        {
            CreateInputPort();
            CreateOutputPort();
        }

        protected override void UpdateTaskIcon()
        {
            taskIconName = "Icons/DecoratorIcon";
            base.UpdateTaskIcon();
        }
    }
}