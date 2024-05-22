using System;

namespace BehaviorDesigner
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TaskNameAttribute : Attribute
    {
        public readonly string name;

        public TaskNameAttribute(string name)
        {
            this.name = name;
        }
    }
}