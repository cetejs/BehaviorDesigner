using System;

namespace BehaviorDesigner
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FieldReadOnlyAttribute : Attribute
    {
    }
}