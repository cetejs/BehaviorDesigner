using System;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedTransform : SharedVariable<Transform>
    {
        public static implicit operator SharedTransform(Transform value)
        {
            return new SharedTransform {Value = value};
        }
    }
}