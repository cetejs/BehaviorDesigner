using System;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedColor : SharedVariable<Color>
    {
        public static implicit operator SharedColor(Color value)
        {
            return new SharedColor {Value = value};
        }
    }
}