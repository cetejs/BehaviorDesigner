using System;
using Object = UnityEngine.Object;

namespace BehaviorDesigner
{
    [Serializable]
    [VariablePriority(1)]
    public class SharedObjectList : SharedList<Object>
    {
    }
}