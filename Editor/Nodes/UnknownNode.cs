namespace BehaviorDesigner.Editor
{
    public class UnknownNode : TaskNode
    {
        protected override bool IsAddComment
        {
            get { return true; }
        }

        public override void Restore()
        {
            base.Restore();
            UnknownTask unknownTask = task as UnknownTask;
            string unknownType = unknownTask.unknownTaskType;
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