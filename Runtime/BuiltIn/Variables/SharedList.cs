using System;
using System.Collections.Generic;

namespace BehaviorDesigner
{
    [Serializable]
    public abstract class SharedList<T> : SharedVariable<List<T>>
    {
        public SharedList()
        {
            value = new List<T>();
        }

        public SharedList(int capacity)
        {
            value = new List<T>(capacity);
        }

        public SharedList(IEnumerable<T> collection)
        {
            value = new List<T>(collection);
        }

        public SharedList(List<T> value)
        {
            this.value = value;
        }
    }
}