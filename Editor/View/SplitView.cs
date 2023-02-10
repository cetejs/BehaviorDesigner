using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits> {}
    }
}