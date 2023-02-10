using System;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedVector3 : SharedVariable<Vector3>
    {
        public static implicit operator SharedVector3(Vector3 value)
        {
            return new SharedVector3 {Value = value};
        }
    }
}