using System;

namespace BehaviorDesigner
{
    [AttributeUsage(AttributeTargets.Class)]
    public class VariablePriorityAttribute : Attribute
    {
        public readonly int priority;

        public VariablePriorityAttribute(int priority)
        {
            this.priority = priority;
        }
    }
}