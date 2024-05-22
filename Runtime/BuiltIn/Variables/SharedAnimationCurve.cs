using System;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedAnimationCurve : SharedVariable<AnimationCurve>
    {
        public static implicit operator SharedAnimationCurve(AnimationCurve value)
        {
            return new SharedAnimationCurve {Value = value};
        }
    }
}