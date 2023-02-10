using System;

namespace BehaviorDesigner
{
    [Serializable]
    public class SharedTag : SharedVariable<Tag>
    {
        public static implicit operator SharedTag(Tag value)
        {
            return new SharedTag {Value = value};
        }
        
        public static implicit operator SharedTag(string value)
        {
            return new SharedTag {Value = value};
        }
    }
}