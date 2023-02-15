namespace BehaviorDesigner
{
    public class Action : Task
    {
        public sealed override void Bind(IBehavior behavior)
        {
            base.Bind(behavior);
        }
        
        public sealed override void Init(Behavior behavior)
        {
            base.Init(behavior);
        }

        public sealed override void Restart()
        {
            base.Restart();
        }
    }
}