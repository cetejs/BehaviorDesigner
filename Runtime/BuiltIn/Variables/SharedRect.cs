using System;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedRect : SharedVariable<Rect>
    {
        public static implicit operator SharedRect(Rect value)
        {
            return new SharedRect {Value = value};
        }
    }
}