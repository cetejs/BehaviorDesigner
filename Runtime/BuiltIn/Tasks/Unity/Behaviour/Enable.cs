using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityBehaviour
{
    [TaskCategory("Behaviour")]
    [TaskName("Enable (Behaviour)")]
    [TaskDescription("Sets enable behaviour.")]
    public class Enable : Action
    {
        [SerializeField]
        private SharedBehaviour behaviour;

        public override void OnStart()
        {
            behaviour.Value.enabled = true;
        }
    }
}