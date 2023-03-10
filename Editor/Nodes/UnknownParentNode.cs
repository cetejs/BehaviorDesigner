namespace BehaviorDesigner.Editor
{
    public class UnknownParentNode : ParentTaskNode
    {
        protected override bool IsAddComment
        {
            get { return true; }
        }

        public override void Restore()
        {
            base.Restore();
            UnknownParentTask unknownParentTask = task as UnknownParentTask;
            string unknownType = unknownParentTask.unknownTaskType;
            int index = unknownType.LastIndexOf('.');
            if (index > 0)
            {
                unknownType = unknownType.Remove(0, index + 1);
            }

            title = $"Unknown {unknownType}";
            
            commentInput.SetEnabled(false);
            commentInput.SetValueWithoutNotify("Unknown Task. Right click and Replace to locate new task.");
            SetComment(commentInput.value);
        }
    }
}