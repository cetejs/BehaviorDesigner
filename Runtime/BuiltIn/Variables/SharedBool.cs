using System;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedBool : SharedVariable<bool>
    {
        public static implicit operator SharedBool(bool value)
        {
            return new SharedBool {Value = value};
        }
    }
}