using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public interface IInspectorGUI
    {
        Task Task { get; }

        void Reset();
        
        void OnGUI(VisualElement container);
    }
}