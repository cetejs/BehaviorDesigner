using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class ActionNode : TaskNode
    {
        protected override bool IsAddComment
        {
            get { return true; }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            AddScriptMenuItem(evt);
            AddReplaceMenuItem(evt);
            base.BuildContextualMenu(evt);
        }
    }
}