using UnityEngine;

namespace BehaviorDesigner.Tasks.Movement
{
    [TaskGroup("Movement")]
    [TaskDescription("Check to see if the any objects are within sight of the agent.")]
    public class CanSeeObject : IntervalConditional
    {
        [SerializeField]
        private Optimization optimization;
        [SerializeField]
        private SharedTag tag = Tag.Untagged;
        [SerializeField]
        private SharedLayerMask objectLayerMask;
        [SerializeField]
        private SharedLayerMask obstacleLayerMask;
        [SerializeField]
        private SharedFloat fieldOfView = 60f;
        [SerializeField]
        private SharedFloat viewDistance = 10f;
        [SerializeField]
        private SharedFloat viewVolume = 1f;
        [SerializeField]
        private SharedFloat angleOffset;
        [SerializeField]
        private SharedVector3 viewOffset;
        [SerializeField]
        private SharedTransform storeResult;
        [SerializeField]
        private bool drawGizmos = true;
        [SerializeField]
        private Color viewColor = new Color(1f, 0.92f, 0.016f, 0.1f);

        private readonly Collider[] hitColliders = new Collider[10];

        public override TaskStatus OnConditionalUpdate()
        {
            float minAngle = float.MaxValue;
            float minDistance = float.MaxValue;
            Transform result = null;
            int length = Physics.OverlapSphereNonAlloc(transform.TransformPoint(viewOffset.Value), viewDistance.Value, hitColliders, objectLayerMask.Value);
            for (int i = 0; i < length; i++)
            {
                Collider hitCollider = hitColliders[i];
                Transform hitTransform = hitCollider.transform;
                if (hitTransform.IsChildOf(transform) || transform.IsChildOf(hitTransform))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(tag.Value))
                {
                    continue;
                }

                if (!hitTransform.CompareTag(tag.Value))
                {
                    continue;
                }

                if (Physics.Linecast(transform.TransformPoint(viewOffset.Value), hitTransform.position, out RaycastHit hit, obstacleLayerMask.Value))
                {
                    if (!hit.transform.IsChildOf(hitTransform) && !hitTransform.IsChildOf(hit.transform))
                    {
                        continue;
                    }
                }

                Vector3 direction = hitTransform.position - transform.position;
                Vector3 forward = Quaternion.AngleAxis(angleOffset.Value, Vector3.up) * transform.forward;
                float angle = Vector3.Angle(forward, direction);
                float distance = direction.sqrMagnitude;
                if (angle > fieldOfView.Value / 2f && distance > viewVolume.Value)
                {
                    continue;
                }

                switch (optimization)
                {
                    case Optimization.Angle:
                        if (minAngle > angle )
                        {
                            minAngle = angle;
                            result = hitTransform;
                        }
                        break;
                    case Optimization.Distance:
                        if (minDistance > distance)
                        {
                            minDistance = distance;
                            result = hitTransform;
                        }
                        break;
                }
            }

            storeResult.Value = result;
            return result ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!drawGizmos)
            {
                return;
            }
            
            Color oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = viewColor;
            float halfFov = fieldOfView.Value * 0.5f;
            Vector3 beginDirection = Quaternion.AngleAxis(-halfFov + angleOffset.Value, Vector3.up) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(transform.TransformPoint(viewOffset.Value), transform.up, beginDirection, fieldOfView.Value, viewDistance.Value);
            UnityEditor.Handles.color = oldColor;
#endif
        }

        public override void OnReset()
        {
            base.OnReset();
            tag = null;
            objectLayerMask = 0;
            obstacleLayerMask = 0;
            fieldOfView = 60f;
            viewDistance = 10f;
            viewVolume = 1f;
            angleOffset = 0f;
            viewOffset = Vector3.zero;
            drawGizmos = true;
            viewColor = new Color(1f, 0.92f, 0.016f, 0.1f);
        }
        
        public enum Optimization
        {
            Angle,
            Distance
        }
    }
}