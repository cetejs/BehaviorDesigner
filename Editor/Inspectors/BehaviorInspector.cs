using UnityEditor;
using UnityEngine;

namespace BehaviorDesigner
{
    public abstract class BehaviorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            IBehavior behavior = (IBehavior) target;
            BehaviorSource source = behavior.Source;
            serializedObject.UpdateIfRequiredOrScript();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"), true);
            GUI.enabled = true;

            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Behavior Name", GUILayout.Width(120f));
            source.behaviorName = EditorGUILayout.TextField(source.behaviorName);
            if (GUILayout.Button("Open"))
            {
                BehaviorWindow.ShowWindow(behavior);
            }

            GUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("source.behaviorDescription"));
            OnDrawInspector();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        protected virtual void OnDrawInspector()
        {
        }

        protected void DrawVariables(IBehavior behavior, SerializedObject serializedBehavior)
        {
            EditorGUI.BeginChangeCheck();
            string key = $"BehaviorDesigner.VariablesFoldout{target.GetHashCode()}";
            bool showVariables = EditorGUILayout.Foldout(EditorPrefs.GetBool(key, true), "Variables");
            if (showVariables)
            {
                ++EditorGUI.indentLevel;
                SerializedProperty serializedVariables = serializedBehavior.FindProperty("source.sharedVariables");
                if (serializedVariables.arraySize == 0)
                {
                    EditorGUILayout.LabelField("There are no variables to display");
                }
                else
                {
                    EditorBehaviorUtility.DrawContentSeparator();
                    for (int i = 0; i < serializedVariables.arraySize; i++)
                    {
                        SerializedProperty serializedVariable = serializedVariables.GetArrayElementAtIndex(i);
                        SerializedProperty serializedName = serializedVariable.FindPropertyRelative("name");
                        SerializedProperty serializedValue = serializedVariable.FindPropertyRelative("value");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(serializedValue, new GUIContent(serializedName.stringValue), true);
                        if (GUILayout.Button(EditorBehaviorUtility.LoadTexture("Icons/VariableDeleteButton"), EditorBehaviorUtility.PlainButtonGUIStyle, GUILayout.Width(18f), GUILayout.Height(18f)))
                        {
                            Undo.RecordObject(behavior.Object, "BehaviorTree Delete Variable");
                            behavior.Source.RemoveVariable(i);
                        }

                        EditorGUILayout.EndHorizontal();
                        EditorBehaviorUtility.DrawContentSeparator();
                    }
                }

                --EditorGUI.indentLevel;
            }

            EditorPrefs.SetBool(key, showVariables);
            if (EditorGUI.EndChangeCheck())
            {
                serializedBehavior.ApplyModifiedProperties();
            }
        }
    }
}