using System;

namespace BehaviorDesigner
{
    [Serializable]
    [VariablePriority(1)]
    public class SharedLongList : SharedList<long>
    {
    }
}