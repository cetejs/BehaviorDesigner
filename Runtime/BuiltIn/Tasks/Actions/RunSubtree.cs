using UnityEngine;

namespace BehaviorDesigner.Tasks
{
    [TaskDescription("Run a new behavior tree.")]
    public class RunSubtree : Action
    {
        [SerializeField]
        private ExternalBehavior behavior;
        [SerializeField]
        private bool isSyncVariables;

        public override void OnStart()
        {
            behavior.Source.Load();
            behavior.Source.Root.Init(owner);
            if (isSyncVariables)
            {
                SyncVariables(owner, behavior);
            }
        }

        public override TaskStatus OnUpdate()
        {
            return behavior.Source.Root.OnUpdate();
        }

        public override void OnEnd()
        {
            if (isSyncVariables)
            {
                SyncVariables(behavior, owner);
            }
        }

        public override void OnReset()
        {
            behavior = null;
            isSyncVariables = false;
        }

        private void SyncVariables(IBehavior b1, IBehavior b2)
        {
            foreach (SharedVariable variable in b1.Source.GetVariables())
            {
                SharedVariable targetVariable = b2.Source.GetVariable(variable.Name);
                if (targetVariable != null)
                {
                    if (targetVariable.GetType() == variable.GetType())
                    {
                        targetVariable.SetValue(variable.GetValue());
                    }
                }
            }
        }
    }
}