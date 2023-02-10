using UnityEngine;

namespace BehaviorDesigner.Tasks
{
    [TaskDescription("Log is a simple task which will output the specified text and return success. It can be used for debugging.")]
    public class Log : Action
    {
        [SerializeField]
        private SharedBool isLogError;
        [SerializeField]
        private SharedString logText;

        public override TaskStatus OnUpdate()
        {
            if (isLogError.Value)
            {
                Debug.LogError(logText.Value);
            }
            else
            {
                Debug.Log(logText.Value);
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            isLogError = false;
            logText = null;
        }
    }
}