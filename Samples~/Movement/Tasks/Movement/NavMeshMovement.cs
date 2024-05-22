using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Tasks.Movement
{
    public abstract class NavMeshMovement : Movement
    {
        [SerializeField]
        protected SharedFloat speed = 10f;
        [SerializeField]
        protected SharedFloat angularSpeed = 120f;
        [SerializeField]
        protected SharedFloat arriveDistance = 0.1f;
        [SerializeField]
        protected SharedBool stopOnTaskEnd = true;
        [SerializeField]
        protected SharedBool updateRotation = true;

        protected NavMeshAgent agent;

        protected override bool HasPath
        {
            get { return agent.hasPath && agent.remainingDistance > arriveDistance.Value; }
        }

        protected override bool HasArrived
        {
            get
            {
                if (agent.pathPending)
                {
                    return false;
                }

                return agent.remainingDistance <= arriveDistance.Value;
            }
        }

        protected override Vector3 Velocity
        {
            get { return agent.velocity; }
        }

        public override void OnAwake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public override void OnStart()
        {
            base.OnStart();
            agent.speed = speed.Value;
            agent.angularSpeed = angularSpeed.Value;
            agent.isStopped = false;
            UpdateRotation(updateRotation.Value);
        }

        public override void OnEnd()
        {
            if (stopOnTaskEnd.Value)
            {
                Stop();
            }
        }

        protected override bool SetDestination(Vector3 target)
        {
            agent.isStopped = false;
            return agent.SetDestination(target);
        }

        protected override void UpdateRotation(bool isUpdate)
        {
            agent.updateRotation = isUpdate;
        }

        protected override void Stop()
        {
            if (agent.hasPath)
            {
                agent.isStopped = true;
            }
        }

        public override void OnReset()
        {
            speed = 10f;
            angularSpeed = 120f;
            arriveDistance = 0.1f;
            stopOnTaskEnd = true;
            updateRotation = true;
        }
    }
}