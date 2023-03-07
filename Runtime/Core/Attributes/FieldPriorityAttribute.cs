using System;

namespace BehaviorDesigner
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FieldPriorityAttribute : Attribute
    {
        public readonly int priority;

        public FieldPriorityAttribute(int priority)
        {
            this.priority = priority;
        }
    }
}