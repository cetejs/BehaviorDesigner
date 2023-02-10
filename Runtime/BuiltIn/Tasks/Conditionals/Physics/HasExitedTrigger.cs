using UnityEngine;

namespace BehaviorDesigner.Tasks
{
    [TaskCategory("Physics")]
    [TaskDescription("Returns success when an object exits the trigger. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
    public class HasExitedTrigger : Conditional
    {
        [SerializeField]
        private SharedTag tag = Tag.Untagged;
        [SerializeField]
        private SharedGameObject collidedGameObject;

        private bool isTriggerExit;

        public override TaskStatus OnUpdate()
        {
            return isTriggerExit ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnEnd()
        {
            isTriggerExit = false;
        }

        public override void OnTriggerExit(Collider other)
        {
            if (!string.IsNullOrEmpty(tag.Value) && other.gameObject.CompareTag(tag.Value))
            {
                collidedGameObject.Value = other.gameObject;
                isTriggerExit = true;
            }
        }

        public override void OnReset()
        {
            tag.Value = Tag.Untagged;
            collidedGameObject = null;
        }
    }
}