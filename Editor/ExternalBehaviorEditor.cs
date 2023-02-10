using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    [CustomEditor(typeof(ExternalBehavior))]
    public class ExternalBehaviorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            bool isChanged = false;
            ExternalBehavior behavior = target as ExternalBehavior;
            BehaviorSource source = behavior.Source;
            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"), true);
            GUI.enabled = true;

            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Behavior Name", GUILayout.Width(120f));
            source.behaviorName = EditorGUILayout.TextField(behavior.Source.behaviorName);
            if (EditorGUI.EndChangeCheck())
            {
                isChanged = true;
            }

            if (GUILayout.Button("Open"))
            {
                BehaviorWindow.ShowWindow(behavior);
            }

            GUILayout.EndHorizontal();
            EditorGUI.BeginChangeCheck();
            source.behaviorDescription = EditorGUILayout.TextArea(source.behaviorDescription, GUILayout.Height(48f));
            source.group = EditorGUILayout.IntField("Group", behavior.Source.group);
            if (EditorGUI.EndChangeCheck())
            {
                isChanged = true;
            }

            if (isChanged)
            {
                EditorUtility.SetDirty(behavior);
            }
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