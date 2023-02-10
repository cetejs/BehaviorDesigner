using System;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    [VariablePriority(1)]
    public class SharedColorList : SharedList<Color>
    {
    }
}