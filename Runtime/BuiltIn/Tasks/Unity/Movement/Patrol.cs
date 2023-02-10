using UnityEngine;

namespace BehaviorDesigner.Tasks.Movement
{
    [TaskDescription("Patrol around the specified waypoints using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    public class Patrol : NavMeshMovement
    {
        [SerializeField]
        private SharedBool isRandomPatrol;
        [SerializeField]
        private SharedBool isReversePatrol;
        [SerializeField]
        private SharedFloat waypointPauseDuration = 1f;
        [SerializeField]
        private SharedTransformList waypoints;

        private int waypointIndex;
        private float waypointPauseTime;

        private Vector3 Target
        {
            get
            {
                if (waypointIndex >= waypoints.Value.Count)
                {
                    return transform.position;
                }

                return waypoints.Value[waypointIndex].position;
            }
        }

        public override void OnStart()
        {
            base.OnStart();
            float minDistance = float.MaxValue;
            for (int i = 0; i < waypoints.Value.Count; i++)
            {
                Transform waypoint = waypoints.Value[i];
                float sqrDistance = (transform.position - waypoint.position).sqrMagnitude;
                if (sqrDistance < minDistance  )
                {
                    minDistance = sqrDistance;
                    waypointIndex = i;
                }   
            }

            waypointPauseTime = -1f;
            SetDestination(Target);
        }

        public override TaskStatus OnUpdate()
        {
            if (waypoints.Value.Count == 0)
            {
                return TaskStatus.Failure;
            }

            if (HasArrived)
            {
                if (waypointPauseTime == -1f)
                {
                    waypointPauseTime = Time.time;
                }
                
                if (Time.time - waypointPauseTime > waypointPauseDuration.Value)
                {
                    waypointPauseTime = -1f;
                    int nextIndex;
                    if (isRandomPatrol.Value)
                    {
                        int randomIndex = Random.Range(0, waypoints.Value.Count);
                        if (randomIndex == waypointIndex)
                        {
                            nextIndex = NextIndex(waypointIndex);
                        }
                        else
                        {
                            nextIndex = randomIndex;
                        }
                    }
                    else
                    {
                        nextIndex = NextIndex(waypointIndex);
                    }

                    if (waypointIndex == nextIndex)
                    {
                        return TaskStatus.Failure;
                    }
                    else
                    {
                        waypointIndex = nextIndex;
                        SetDestination(Target);
                    }
                }
            }

            return TaskStatus.Running;
        }

        private int NextIndex(int index)
        {
            index += isReversePatrol.Value ? -1 : 1;

            if (index >= waypoints.Value.Count)
            {
                index = 0;
            }
            else if (index < 0)
            {
                index = waypoints.Value.Count - 1;
            }

            return index;
        }

        public override void OnDrawGizmos()
        {
            Color oldColor = Gizmos.color;
            for (int i = 0; i < waypoints.Value.Count; i++)
            {
                Transform waypoint = waypoints.Value[i];
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(waypoint.position, 0.5f);
                if (!isRandomPatrol.Value)
                {
                    Gizmos.color = Color.green;
                    Transform nextWaypoint = waypoints.Value[NextIndex(i)];
                    Gizmos.DrawLine(waypoint.position, nextWaypoint.position);
                }
            }

            Gizmos.color = oldColor;
        }

        public override void OnReset()
        {
            base.OnReset();
            isRandomPatrol = false;
            waypointPauseDuration = 1f;
            waypoints = null;
        }
    }
}