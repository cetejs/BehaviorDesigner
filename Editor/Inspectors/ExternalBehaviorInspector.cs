using UnityEditor;
using UnityEditor.Callbacks;

namespace BehaviorDesigner
{
    [CustomEditor(typeof(ExternalBehavior))]
    public class ExternalBehaviorInspector : BehaviorInspector
    {
        protected override void OnDrawInspector()
        {
            IBehavior behavior = (IBehavior) target;
            DrawVariables(behavior, serializedObject);
        }

        [OnOpenAsset]
        public static bool ClickAction(int instanceID, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is ExternalBehavior behavior)
            {
                BehaviorWindow.ShowWindow(behavior);
                return true;
            }

            return false;
        }
    }
}