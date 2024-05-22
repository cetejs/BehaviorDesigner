using UnityEngine;

namespace BehaviorDesigner.Tasks.Movement
{
    public abstract class Movement : Action
    {
        protected abstract bool HasPath { get; }

        protected abstract bool HasArrived { get; }

        protected abstract Vector3 Velocity { get; }

        protected abstract bool SetDestination(Vector3 target);

        protected abstract void UpdateRotation(bool isUpdate);

        protected abstract void Stop();
    }
}