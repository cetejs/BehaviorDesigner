using UnityEngine;

namespace BehaviorDesigner.Tasks
{
    [TaskCategory("Physics")]
    [TaskDescription("Returns success when an object enters the trigger. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
    public class HasEnteredTrigger : Conditional
    {
        [SerializeField]
        private SharedTag tag = Tag.Untagged;
        [SerializeField]
        private SharedGameObject collidedGameObject;

        private bool isTriggerEnter;

        public override TaskStatus OnUpdate()
        {
            return isTriggerEnter ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnEnd()
        {
            isTriggerEnter = false;
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(tag.Value) && other.gameObject.CompareTag(tag.Value))
            {
                collidedGameObject.Value = other.gameObject;
                isTriggerEnter = true;
            }
        }

        public override void OnReset()
        {
            tag.Value = Tag.Untagged;
            collidedGameObject = null;
        }
    }
}