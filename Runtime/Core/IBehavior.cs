using UnityEngine;

namespace BehaviorDesigner
{
    public interface IBehavior
    {
        Root Root { get; }

        BehaviorSource Source { get; }

        int InstanceID { get; }

        Object Object { get; }

        void BindVariables(Task task);
    }
}