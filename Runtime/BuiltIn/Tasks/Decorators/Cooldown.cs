using UnityEngine;

namespace BehaviorDesigner
{
    [TaskIcon("Icons/CooldownIcon")]
    [TaskDescription("Waits the specified duration after the child has completed before returning the child's status of success or failure.")]
    public class Cooldown : Decorator
    {
        [SerializeField]
        private SharedFloat duration = 1f;
        private float cooldownTime;

        public bool IsCooling
        {
            get { return Time.time - cooldownTime < duration.Value; }
        }

        public override bool CanExecute
        {
            get { return !IsCooling && base.CanExecute; }
        }

        public override void OnStart()
        {
            base.OnStart();
            cooldownTime = Time.time;
        }

        public override TaskStatus OnDecorate(TaskStatus status)
        {
            if (IsCooling)
            {
                return TaskStatus.Running;
            }

            return status;
        }

        public override void OnReset()
        {
            duration = 1f;
        }
    }
}