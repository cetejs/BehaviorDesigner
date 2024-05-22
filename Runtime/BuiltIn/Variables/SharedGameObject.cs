using System;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedGameObject : SharedVariable<GameObject>
    {
        public static implicit operator SharedGameObject(GameObject value)
        {
            return new SharedGameObject {Value = value};
        }
    }
}