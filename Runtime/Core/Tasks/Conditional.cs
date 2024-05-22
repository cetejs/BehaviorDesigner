namespace BehaviorDesigner
{
    public class Conditional : Task
    {
        public sealed override void Bind(BehaviorSource source)
        {
            base.Bind(source);
        }

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