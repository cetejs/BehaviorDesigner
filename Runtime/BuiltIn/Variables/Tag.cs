using System;
using UnityEngine;

namespace BehaviorDesigner
{
    [Serializable]
    public struct Tag
    {
        public static readonly string Untagged = "Untagged";
        
        [SerializeField]
        private string value;

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public static implicit operator Tag(string value)
        {
            return new Tag {Value = value};
        }

        public static implicit operator string(Tag tag)
        {
            return tag.value;
        }
    }
}