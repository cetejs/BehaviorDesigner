using UnityEngine;

namespace BehaviorDesigner.Tasks
{
    [TaskCategory("Physics")]
    [TaskDescription("Returns success when a collision ends. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
    public class HasExitedCollision : Conditional
    {
        [SerializeField]
        private SharedTag tag = Tag.Untagged;
        [SerializeField]
        private SharedGameObject collidedGameObject;

        private bool isCollisionExit;

        public override TaskStatus OnUpdate()
        {
            return isCollisionExit ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnEnd()
        {
            isCollisionExit = false;
        }

        public override void OnCollisionEnter(Collision collision)
        {
            if (!string.IsNullOrEmpty(tag.Value) && collision.gameObject.CompareTag(tag.Value))
            {
                collidedGameObject.Value = collision.gameObject;
                isCollisionExit = true;
            }
        }

        public override void OnReset()
        {
            tag.Value = Tag.Untagged;
            collidedGameObject = null;
        }
    }
}