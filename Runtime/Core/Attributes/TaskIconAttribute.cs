using System;

namespace BehaviorDesigner
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TaskIconAttribute : Attribute
    {
        public readonly string iconName;

        public TaskIconAttribute(string iconName)
        {
            this.iconName = iconName;
        }
    }
}