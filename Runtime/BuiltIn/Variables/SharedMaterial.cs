using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedMaterial : SharedVariable<Material>
    {
        public static implicit operator SharedMaterial(Material value)
        {
            return new SharedMaterial {Value = value};
        }
    }
}