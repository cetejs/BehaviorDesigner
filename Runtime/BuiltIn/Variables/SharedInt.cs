using System;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedInt : SharedVariable<int>
    {
        public static implicit operator SharedInt(int value)
        {
            return new SharedInt {Value = value};
        }
    }
}