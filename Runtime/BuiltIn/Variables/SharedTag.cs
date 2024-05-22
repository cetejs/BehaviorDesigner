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

    [Serializable]
    public struct Tag
    {
        public static readonly string Untagged = "Untagged";

        public string value;

        public static implicit operator Tag(string value)
        {
            return new Tag {value = value};
        }

        public static implicit operator string(Tag tag)
        {
            return tag.value;
        }
    }
}