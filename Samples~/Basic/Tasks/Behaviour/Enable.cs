using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityBehaviour
{
    [TaskGroup("Behaviour")]
    [TaskName("Enable (Behaviour)")]
    [TaskDescription("Sets enable behaviour.")]
    public class Enable : Action
    {
        [SerializeField]
        private SharedBehaviour behaviour;

        public override TaskStatus OnUpdate()
        {
            behaviour.Value.enabled = true;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            behaviour = null;
        }
    }
}