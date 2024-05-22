using System;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedLong : SharedVariable<long>
    {
        public static implicit operator SharedLong(long value)
        {
            return new SharedLong {Value = value};
        }
    }
}