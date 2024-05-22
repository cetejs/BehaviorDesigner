namespace BehaviorDesigner
{
    [TaskIcon("Icons/IdleIcon")]
    [TaskDescription("Returns a TaskStatus of running. Will only stop when interrupted or a conditional abort is triggered.")]
    public class Idle : Action
    {
        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
        }
    }
}