using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class ConditionalNode : TaskNode
    {
        protected override bool IsAddComment
        {
            get { return true; }
        }
        
        public ConditionalNode()
        {
            inputContainer.style.backgroundColor = new Color(1f, 0.8f, 0.16f, 1f);
        }
        
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            AddScriptMenuItem(evt);
            AddBreakpointMenuItem(evt);
            AddReplaceMenuItem(evt);
            base.BuildContextualMenu(evt);
        }

        protected override void OnTaskUpdate(TaskStatus status)
        {
            base.OnTaskUpdate(status);
            Color color = inputContainer.style.backgroundColor.value;
            color.a = 0.5f + Mathf.PingPong(Time.time, 0.5f);
            inputContainer.style.backgroundColor = color;
        }
    }
}