using System;

namespace BehaviorDesigner
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TaskDescriptionAttribute : Attribute
    {
        public readonly string description;

        public TaskDescriptionAttribute(string description)
        {
            this.description = description;
        }
    }
}