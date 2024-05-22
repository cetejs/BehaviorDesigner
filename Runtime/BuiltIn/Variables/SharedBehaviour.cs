using System;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedBehaviour : SharedVariable<Behaviour>
    {
        public static implicit operator SharedBehaviour(Behaviour value)
        {
            return new SharedBehaviour {Value = value};
        }
    }
}