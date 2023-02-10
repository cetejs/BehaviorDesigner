using System;

namespace BehaviorDesigner.Editor
{
    public class TaskNodeFactory
    {
        public TaskNode Create(Task task, BehaviorWindow window)
        {
            Type type = task.GetType();
            TaskNode node;
            if (type.IsSubclassOf(typeof(Composite)))
            {
                node = new CompositeNode();
            }
            else if (type.IsSubclassOf(typeof(Conditional)))
            {
                node = new ConditionalNode();
            }
            else if (type.IsSubclassOf(typeof(Decorator)))
            {
                node = new DecoratorNode();
            }
            else if (type.IsSubclassOf(typeof(Root)))
            {
                node = new RootNode();
            }
            else
            {
                node = new ActionNode();
            }

            node.Init(task, window);
            node.Restore();
            return node;
        }
    }
}