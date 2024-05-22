using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace BehaviorDesigner
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }

        private BehaviorWindow window;
        private IMGUIContainer container;
        private ToolbarMenu toolbarMenu;
        private Editor editor;
        private Task selectedTask;

        public void Init(BehaviorWindow window)
        {
            this.window = window;
            container = new IMGUIContainer(() =>
            {
                if (editor != null && editor.target != null)
                {
                    try
                    {
                        DrawInspector();
                    }
                    catch (ExitGUIException)
                    {
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            });

            this.Q<ScrollView>().Add(container);
            RegisterToolbarMenu();
        }

        public void Refresh()
        {
            if (editor != null)
            {
                Object.DestroyImmediate(editor);
                editor = null;
            }

            if (window.Behavior != null)
            {
                editor = Editor.CreateEditor(window.Behavior.Object);
            }

            OnNodeUnselected(null);
        }

        public void OnNodeSelected(TaskNode node)
        {
            selectedTask = node.Task;
            toolbarMenu.visible = true;
        }

        public void OnNodeUnselected(TaskNode node)
        {
            selectedTask = null;
            toolbarMenu.visible = false;
        }

        private void RegisterToolbarMenu()
        {
            toolbarMenu = parent.Q<ToolbarMenu>();
            toolbarMenu.ElementAt(1).style.backgroundImage = EditorBehaviorUtility.LoadTexture("Icons/GearIcon");
            toolbarMenu.menu.AppendAction("Edit Script", action =>
            {
                if (selectedTask != null)
                {
                    EditorBehaviorUtility.OpenScript(selectedTask);
                }
            });

            toolbarMenu.menu.AppendAction("Locate Script", action =>
            {
                if (selectedTask != null)
                {
                    EditorBehaviorUtility.SelectScript(selectedTask);
                }
            });

            toolbarMenu.menu.AppendAction("Reset", action =>
            {
                if (selectedTask != null)
                {
                    Undo.RecordObject(window.Behavior.Object, "BehaviorTree Reset Task");
                    selectedTask.OnReset();
                }
            });
        }

        private void DrawInspector()
        {
            if (selectedTask == null)
            {
                return;
            }

            SerializedObject serializedObject = editor.serializedObject;
            serializedObject.UpdateIfRequiredOrScript();
            EditorGUI.BeginChangeCheck();
            SerializedProperty serializedTasks = serializedObject.FindProperty("source.tasks");
            for (int i = 0; i < serializedTasks.arraySize; i++)
            {
                SerializedProperty serializedTask = serializedTasks.GetArrayElementAtIndex(i);
                Task task = (Task) serializedTask.managedReferenceValue;
                if (task.Guid == selectedTask.Guid)
                {
                    selectedTask = task;
                    int depth = serializedTask.depth;
                    for (bool enterChildren = true; serializedTask.NextVisible(enterChildren) && serializedTask.depth > depth; enterChildren = false)
                    {
                        Type type = window.SharedVariableTypes.Find(t => t.Name == serializedTask.type);
                        if (type != null)
                        {
                            DrawSharedVariable(serializedTask, type);
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(serializedTask, new GUIContent(serializedTask.displayName), true);
                        }
                    }
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawSharedVariable(SerializedProperty serializedVariable, Type type)
        {
            SerializedProperty serializedName = serializedVariable.FindPropertyRelative("name");
            SerializedProperty serializedValue = serializedVariable.FindPropertyRelative("value");
            SerializedProperty serializedIsShared = serializedVariable.FindPropertyRelative("isShared");
            EditorGUILayout.BeginHorizontal();
            GUILayoutOption width = GUILayout.Width(container.layout.width - 25f);
            if (serializedIsShared.boolValue)
            {
                List<string> sharedNames = new List<string>();
                sharedNames.Add("{None}");
                foreach (SharedVariable variable in window.Source.Variables)
                {
                    if (variable.GetType() == type)
                    {
                        sharedNames.Add(variable.Name);
                    }
                }

                Color backgroundColor = GUI.backgroundColor;
                int index = sharedNames.IndexOf(serializedName.stringValue);
                if (index <= 0)
                {
                    index = 0;
                    GUI.backgroundColor = Color.red;
                }

                index = EditorGUILayout.Popup(serializedVariable.displayName, index, sharedNames.ToArray(), width);
                serializedName.stringValue = sharedNames[index];
                GUI.backgroundColor = backgroundColor;
            }
            else
            {
                EditorGUILayout.PropertyField(serializedValue, new GUIContent(serializedVariable.displayName), true, width);
            }

            string variableButtonName = serializedIsShared.boolValue ? "Icons/VariableButtonSelected" : "Icons/VariableButton";
            if (GUILayout.Button(EditorBehaviorUtility.LoadTexture(variableButtonName), EditorBehaviorUtility.PlainButtonGUIStyle, GUILayout.Width(18f), GUILayout.Height(18f)))
            {
                serializedIsShared.boolValue = !serializedIsShared.boolValue;
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}