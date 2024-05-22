using UnityEngine;

namespace BehaviorDesigner
{
    [TaskIcon("Icons/LogIcon")]
    [TaskDescription("Log is a simple task which will output the specified text and return success. It can be used for debugging.")]
    public class Log : Action
    {
        [SerializeField]
        private SharedBool logError;
        [SerializeField]
        private SharedString logText;

        public override TaskStatus OnUpdate()
        {
            if (logError.Value)
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
            logError = false;
            logText = null;
        }
    }
}