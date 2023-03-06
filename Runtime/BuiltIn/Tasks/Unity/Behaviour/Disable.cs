using UnityEngine;

namespace BehaviorDesigner.Tasks.UnityBehaviour
{
    [TaskCategory("Behaviour")]
    [TaskName("Disable (Behaviour)")]
    [TaskDescription("Sets disable behaviour.")]
    public class Disable : Action
    {
        [SerializeField]
        private SharedBehaviour behaviour;

        public override void OnStart()
        {
            base.OnStart();
            behaviour.Value.enabled = false;
        }
    }
}