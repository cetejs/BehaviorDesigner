using System;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    [VariablePriority(1)]
    public class SharedVector2IntList : SharedList<Vector2Int>
    {
    }
}