using System;

namespace BehaviorDesigner
{
    [Serializable]
    [VariablePriority(1)]
    public class SharedStringList : SharedList<string>
    {
    }
}