using UnityEngine;

namespace BehaviorDesigner
{
    public interface IBehavior
    {
        public Object Object { get; }

        public BehaviorSource Source { get; }
    }
}