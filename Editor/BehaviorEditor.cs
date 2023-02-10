using UnityEditor;
using UnityEngine;

namespace BehaviorDesigner.Editor
{
    [CustomEditor(typeof(Behavior), true)]
    public class BehaviorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            bool isChanged = false;
            Behavior behavior = target as Behavior;
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
            SerializedProperty property = serializedObject.FindProperty("external");
            ExternalBehavior oldExternal = property.objectReferenceValue as ExternalBehavior;
            EditorGUILayout.PropertyField(property, new GUIContent("External Behavior"), true);
            source.group = EditorGUILayout.IntField("Group", behavior.Source.group);
            if (EditorGUI.EndChangeCheck())
            {
                isChanged = true;
            }

            string key = "BehaviorDesign.OptionsFoldout." + BehaviorUtils.GetFileId(behavior);;
            bool showOptions = EditorGUILayout.Foldout(EditorPrefs.GetBool(key, true), "Options");
            if (showOptions)
            {
                EditorGUI.BeginChangeCheck();
                ++EditorGUI.indentLevel;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("updateType"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("restartWhenComplete"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("resetValuesOnRestart"), true);
                --EditorGUI.indentLevel;
                if (EditorGUI.EndChangeCheck())
                {
                    isChanged = true;
                }
            }

            EditorPrefs.SetBool(key, showOptions);
            if (isChanged)
            {
                serializedObject.ApplyModifiedProperties();
                ExternalBehavior newExternal = property.objectReferenceValue as ExternalBehavior;
                if (oldExternal != newExternal && newExternal)
                {
                    behavior.ClearSource();
                }

                EditorUtility.SetDirty(behavior);
                if (oldExternal != newExternal && EditorWindow.HasOpenInstances<BehaviorWindow>())
                {
                    BehaviorWindow.ShowWindow(behavior);
                }
            }
        }
    }
}