using UnityEngine;

namespace BehaviorDesigner
{
    [TaskDescription("Run a new behavior tree.")]
    public class RunSubtree : Action
    {
        [SerializeField]
        private ExternalBehavior behavior;
        [SerializeField]
        private bool syncVariables;
        [SerializeField]
        private bool resetVariables;

        private Root root;
        private bool isInit;

        public override void OnStart()
        {
            if (behavior == null)
            {
                return;
            }

            if (!isInit)
            {
                behavior = Object.Instantiate(behavior);
                behavior.Source.UpdateVariables();
                root = behavior.Source.root;
                root.Init(owner);
                root.Bind(behavior.Source);
                isInit = true;
            }
            else if (resetVariables)
            {
                behavior.Source.ReloadVariables(behavior.Source);
            }

            if (syncVariables)
            {
                SyncVariables(owner.Source, behavior.Source);
            }

            root.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            if (!isInit)
            {
                return TaskStatus.Failure;
            }

            return root.OnUpdate();
        }

        public override void OnEnd()
        {
            if (!isInit)
            {
                return;
            }

            if (syncVariables)
            {
                SyncVariables(behavior.Source, owner.Source);
            }
        }

        public override void OnReset()
        {
            behavior = null;
            syncVariables = false;
            resetVariables = false;
        }

        private void SyncVariables(BehaviorSource s1, BehaviorSource s2)
        {
            foreach (SharedVariable variable in s1.Variables)
            {
                SharedVariable targetVariable = s2.GetVariable(variable.Name);
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