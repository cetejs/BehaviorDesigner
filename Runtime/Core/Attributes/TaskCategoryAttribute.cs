using System;

namespace BehaviorDesigner
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TaskCategoryAttribute : Attribute
    {
        public readonly string category;

        public TaskCategoryAttribute(string category)
        {
            this.category = category;
        }
    }
}