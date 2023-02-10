using System;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    [VariablePriority(1)]
    public class SharedQuaternionList : SharedList<Quaternion>
    {
    }
}