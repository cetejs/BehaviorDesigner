using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class BlackboardVariableField : BlackboardField
    {
        public BlackboardRow Row { get; set; }
        public Action<BlackboardVariableField> onDeleteCallback;

        public BlackboardVariableField(Texture icon, string text, string typeText) : base(icon, text, typeText)
        {
        }

        protected override void BuildFieldContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildFieldContextualMenu(evt);
            evt.menu.AppendAction("Delete", delete =>
            {
                onDeleteCallback?.Invoke(this);
            }, DropdownMenuAction.AlwaysEnabled);
        }
    }
}