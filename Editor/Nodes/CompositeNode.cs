using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class CompositeNode : ParentTaskNode
    {
        private VisualElement abortType;
        private AbortType lastAbortType;

        protected override bool IsAddComment
        {
            get { return true; }
        }

        public CompositeNode()
        {
            abortType = this.Q("abort-type");
        }

        public override void Init(Task task, BehaviorWindow window)
        {
            base.Init(task, window);
            window.onSave += UpdateAbortType;
        }

        public override void Restore()
        {
            base.Restore();
            UpdateAbortType();
        }

        private void UpdateAbortType()
        {
            Composite composite = task as Composite;
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

            abortType.visible = true;
            switch (composite.AbortType)
            {
                case AbortType.Self:
                    abortType.style.backgroundImage = BehaviorUtils.Load<Texture2D>("Icons/AbortSelf");
                    break;
                case AbortType.LowerPriority:
                    abortType.style.backgroundImage = BehaviorUtils.Load<Texture2D>("Icons/AbortLowerPriority");
                    break;
                case AbortType.Both:
                    abortType.style.backgroundImage = BehaviorUtils.Load<Texture2D>("Icons/AbortBoth");
                    break;
            }
        }
    }
}