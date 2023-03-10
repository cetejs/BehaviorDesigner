using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class AddMenuWindowProvider : MenuWindowProvider
    {
        public override string Title
        {
            get { return "Add Task"; }
        }

        public override bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            window.RegisterUndo("Add Task");
            Type type = searchTreeEntry.userData as Type;
            Task task = Activator.CreateInstance(type) as Task;
            task.Id = window.Source.NewTaskId();
            TaskNode node = window.CreateNode(task);
            Vector2 worldMousePos = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent, context.screenMousePosition - window.position.position);
            Vector2 localMousePos = window.View.contentViewContainer.WorldToLocal(worldMousePos);
            node.SetPosition(new Rect(localMousePos, new Vector2(100f, 100f)));
            window.View.AddToSelection(node);
            window.View.AddElement(node);
            window.Save();
            return true;
        }
    }
}