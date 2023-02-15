using UnityEngine;

namespace BehaviorDesigner
{
    public class ExternalBehavior : ScriptableObject, IBehavior
    {
        [SerializeField]
        private BehaviorSource source;

        public Root Root
        {
            get { return Source.Root; }
        }

        public BehaviorSource Source
        {
            get
            {
                if (source == null)
                {
                    source = new BehaviorSource();
                }

                return source;
            }
        }

        public int InstanceID
        {
            get { return GetInstanceID(); }
        }

        public Object Object
        {
            get { return this; }
        }

        public void BindVariables(Task task)
        {
            Source.BindVariables(task);
        }
    }
}