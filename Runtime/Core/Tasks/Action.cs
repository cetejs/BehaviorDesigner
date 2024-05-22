namespace BehaviorDesigner
{
    public class Action : Task
    {
        public sealed override void Init(BehaviorTree behavior)
        {
            base.Init(behavior);
        }

        public sealed override TaskStatus OnUpdate(bool isUpdateAbort)
        {
            return base.OnUpdate(isUpdateAbort);
        }
    }
}