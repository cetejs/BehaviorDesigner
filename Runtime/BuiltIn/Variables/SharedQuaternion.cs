using System;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedQuaternion : SharedVariable<Quaternion>
    {
        public static implicit operator SharedQuaternion(Quaternion value)
        {
            return new SharedQuaternion {Value = value};
        }
    }
}