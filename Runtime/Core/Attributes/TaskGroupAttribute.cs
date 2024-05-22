using System;

namespace BehaviorDesigner
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TaskGroupAttribute : Attribute
    {
        public readonly string group;

        public TaskGroupAttribute(string group)
        {
            this.group = group;
        }
    }
}