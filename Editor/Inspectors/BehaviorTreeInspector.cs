using UnityEditor;

namespace BehaviorDesigner
{
    [CustomEditor(typeof(BehaviorTree))]
    public class BehaviorTreeInspector : BehaviorInspector
    {
        private static Editor externalBehaviorEditor;

        protected override void OnDrawInspector()
        {
            BehaviorTree behavior = (BehaviorTree) target;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("externalBehavior"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if (behavior.ExternalBehavior != null)
                {
                    BehaviorWindow.ShowWindow(behavior.ExternalBehavior);
                }
                else
                {
                    BehaviorWindow.ShowWindow(behavior);
                }
            }

            if (behavior.ExternalBehavior != null)
            {
                CreateCachedEditor(behavior.ExternalBehavior, null, ref externalBehaviorEditor);
                externalBehaviorEditor.serializedObject.UpdateIfRequiredOrScript();
                DrawVariables(behavior.ExternalBehavior, externalBehaviorEditor.serializedObject);
            }
            else
            {
                DrawVariables(behavior, serializedObject);
            }

            string key = $"BehaviorDesigner.OptionsFoldout{target.GetHashCode()}";
            bool showOptions = EditorGUILayout.Foldout(EditorPrefs.GetBool(key, true), "Options");
            if (showOptions)
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("updateType"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("restartWhenComplete"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("resetValuesOnRestart"));
                --EditorGUI.indentLevel;
            }

            EditorPrefs.SetBool(key, showOptions);
        }
    }
}