using UnityEngine;

namespace BehaviorDesigner
{
    public class ExternalBehavior : ScriptableObject, IBehavior
    {
        [SerializeField]
        private BehaviorSource source = new BehaviorSource();

        public Object Object
        {
            get { return this; }
        }

        public BehaviorSource Source
        {
            get { return source; }
            set { source = value; }
        }

        public ExternalBehavior Clone()
        {
            return Instantiate(this);
        }
    }
}