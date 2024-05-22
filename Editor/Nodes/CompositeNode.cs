using UnityEngine.UIElements;

namespace BehaviorDesigner
{
    public class CompositeNode : ParentTaskNode
    {
        private Composite composite;
        private VisualElement abortType;
        private AbortType lastAbortType;

        public CompositeNode()
        {
            abortType = this.Q("abort-type");
        }

        public override void Init(Task task, BehaviorWindow window)
        {
            base.Init(task, window);
            composite = task as Composite;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (!visible)
            {
                return;
            }

            UpdateAbortType();
        }

        protected override void OnVisible()
        {
            base.OnVisible();
            abortType.visible = visible && composite.AbortType != AbortType.None;
        }

        protected override void CreatePorts()
        {
            CreateInputPort();
            CreateOutputPort();
        }

        protected override void UpdateTaskIcon()
        {
            taskIconName = "Icons/CompositeIcon";
            base.UpdateTaskIcon();
        }

        public override void Restore()
        {
            base.Restore();
            UpdateAbortType();
        }

        private void UpdateAbortType()
        {
            if (lastAbortType == composite.AbortType)
            {
                return;
            }

            lastAbortType = composite.AbortType;
            if (composite.AbortType == AbortType.None)
            {
                abortType.visible = false;
                return;
            }

            abortType.visible = visible;
            switch (composite.AbortType)
            {
                case AbortType.Self:
                    abortType.style.backgroundImage = EditorBehaviorUtility.LoadTexture("Icons/AbortSelfIcon");
                    break;
                case AbortType.LowerPriority:
                    abortType.style.backgroundImage = EditorBehaviorUtility.LoadTexture("Icons/AbortLowerPriorityIcon");
                    break;
                case AbortType.Both:
                    abortType.style.backgroundImage = EditorBehaviorUtility.LoadTexture("Icons/AbortBothIcon");
                    break;
            }
        }
    }
}