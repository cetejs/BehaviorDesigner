using System;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    [VariablePriority(1)]
    public class SharedVector3List : SharedList<Vector3>
    {
    }
}