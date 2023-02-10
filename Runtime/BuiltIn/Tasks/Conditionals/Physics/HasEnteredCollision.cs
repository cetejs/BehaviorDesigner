using UnityEngine;

namespace BehaviorDesigner.Tasks
{
    [TaskCategory("Physics")]
    [TaskDescription("Returns success when a collision starts. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
    public class HasEnteredCollision : Conditional
    {
        [SerializeField]
        private SharedTag tag = Tag.Untagged;
        [SerializeField]
        private SharedGameObject collidedGameObject;

        private bool isCollisionEnter;

        public override TaskStatus OnUpdate()
        {
            return isCollisionEnter ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnEnd()
        {
            isCollisionEnter = false;
        }

        public override void OnCollisionEnter(Collision collision)
        {
            if (!string.IsNullOrEmpty(tag.Value) && collision.gameObject.CompareTag(tag.Value))
            {
                collidedGameObject.Value = collision.gameObject;
                isCollisionEnter = true;
            }
        }

        public override void OnReset()
        {
            tag.Value = Tag.Untagged;
            collidedGameObject = null;
        }
    }
}