using UnityEngine.UIElements;

namespace BehaviorDesigner
{
    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits> {}
    }
}