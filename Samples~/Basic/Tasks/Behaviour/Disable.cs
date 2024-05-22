using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityBehaviour
{
    [TaskGroup("Behaviour")]
    [TaskName("Disable (Behaviour)")]
    [TaskDescription("Sets disable behaviour.")]
    public class Disable : Action
    {
        [SerializeField]
        private SharedBehaviour behaviour;

        public override TaskStatus OnUpdate()
        {
            behaviour.Value.enabled = false;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            behaviour = null;
        }
    }
}