using System;
using Object = UnityEngine.Object;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedObject : SharedVariable<Object>
    {
        public static implicit operator SharedObject(Object value)
        {
            return new SharedObject {Value = value};
        }
    }
}