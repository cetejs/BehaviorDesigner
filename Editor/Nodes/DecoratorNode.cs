namespace BehaviorDesigner.Editor
{
    public class DecoratorNode : ParentTaskNode
    {
        protected override bool IsAddComment
        {
            get { return true; }
        }
    }
}