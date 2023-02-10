using System;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedString : SharedVariable<string>
    {
        public static implicit operator SharedString(string value)
        {
            return new SharedString {Value = value};
        }
    }
}