using UnityEngine;

namespace BehaviorDesigner
{
    public abstract class IntervalConditional : Conditional
    {
        [SerializeField]
        protected SharedFloat interval;

        private float time;
        private TaskStatus lastStatus;

        public sealed override TaskStatus OnUpdate()
        {
            if (interval.Value <= 0)
            {
                return OnConditionalUpdate();
            }

            if (Time.time - time <= interval.Value && lastStatus != TaskStatus.Inactive)
            {
                return lastStatus;
            }

            TaskStatus status = OnConditionalUpdate();
            if (lastStatus != status)
            {
                lastStatus = status;
                time = Time.time;
            }

            return status;
        }

        public virtual TaskStatus OnConditionalUpdate()
        {
            return base.OnUpdate();
        }

        public override void OnReset()
        {
            interval = 0f;
        }
    }
}