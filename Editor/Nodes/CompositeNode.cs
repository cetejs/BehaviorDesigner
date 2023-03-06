using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class CompositeNode : ParentTaskNode
    {
        protected override bool IsAddComment
        {
            get { return true; }
        }
        
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            AddScriptMenuItem(evt);
            AddBreakpointMenuItem(evt);
            AddReplaceMenuItem(evt);
            base.BuildContextualMenu(evt);
        }
    }
}