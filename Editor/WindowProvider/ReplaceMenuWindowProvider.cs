using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace BehaviorDesigner.Editor
{
    public class ReplaceMenuWindowProvider : MenuWindowProvider
    {
        private TaskNode node;

        public override string Title
        {
            get { return "Replace Task"; }
        }

        public void Init(BehaviorWindow window, TaskNode node)
        {
            Init(window);
            this.node = node;
        }

        public override bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            window.RegisterUndo("Replace TaskNode");
            Type type = searchTreeEntry.userData as Type;
            Task task = Activator.CreateInstance(type) as Task;
            task.Id = window.Source.NewTaskId();
            TaskNode newNode = window.CreateNode(task);
            node.Replace(newNode);
            IEnumerable<GraphElement> elementsToRemove = node.CollectElements();
            window.View.DeleteElements(elementsToRemove);
            window.View.AddElement(newNode);
            window.Save();
            return true;
        }
    }
}