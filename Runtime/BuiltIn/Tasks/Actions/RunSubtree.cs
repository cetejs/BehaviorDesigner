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
        [SerializeField]
        private bool isResetVariables;

        private BehaviorSource source;
        private Root root;
        private bool isRestart;

        public override void OnStart()
        {
            source = behavior.Source;
            if (!isRestart)
            {
                source.Load();
                root = source.Root;
                root.Bind(behavior);
                root.Init(owner);
            }
            else if (isResetVariables)
            {
                source.ReloadVariables();
                root.Bind(behavior);
            }

            if (isSyncVariables)
            {
                SyncVariables(owner, behavior);
            }

            root.OnStart();
            isRestart = true;
        }

        public override TaskStatus OnUpdate()
        {
            return root.OnUpdate();
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
            isResetVariables = false;
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