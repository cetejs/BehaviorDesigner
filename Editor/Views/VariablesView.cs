using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace BehaviorDesigner
{
    public class VariablesView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<VariablesView, UxmlTraits> {}

        private BehaviorWindow window;
        private ToolbarMenu toolbarMenu;
        private TextField nameInput;
        private DropdownField typeDp;
        private Button addBtn;
        private IMGUIContainer container;
        private Editor editor;
        private List<string> variableTypeChoices;

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
                    catch (Exception e)
                    {
                        if (e is ExitGUIException)
                        {
                            return;
                        }

                        Debug.LogError(e);
                    }
                }
            });

            Undo.undoRedoPerformed += container.MarkDirtyRepaint;
            this.Q<ScrollView>().Add(container);
            RegisterMenu();
        }

        public void Refresh()
        {
            bool display = window != null && window.Source != null;
            this.Q("tool-field").style.display = display ? DisplayStyle.Flex : DisplayStyle.None;
            if (editor != null)
            {
                Object.DestroyImmediate(editor);
                editor = null;
            }

            if (display)
            {
                editor = Editor.CreateEditor(window.Behavior.Object);
            }
        }

        private void RegisterMenu()
        {
            variableTypeChoices = new List<string>(window.SharedVariableTypes.Count);
            foreach (Type type in window.SharedVariableTypes)
            {
                variableTypeChoices.Add(type.Name.Replace("Shared", ""));
            }

            nameInput = this.Q<TextField>("name-input");
            nameInput.RegisterValueChangedCallback(evt =>
            {
                RefreshAddButton();
            });

            typeDp = this.Q<DropdownField>("type-dp");
            typeDp.choices = variableTypeChoices;
            typeDp.index = 0;

            addBtn = this.Q<Button>("add-btn");
            addBtn.SetEnabled(false);
            addBtn.clickable.clicked += AddVariable;
        }

        private void AddVariable()
        {
            Type type = window.SharedVariableTypes[typeDp.index];
            SharedVariable variable = (SharedVariable) Activator.CreateInstance(type);
            variable.Name = nameInput.value;
            addBtn.SetEnabled(false);
            Undo.RecordObject(window.Behavior.Object, "BehaviorTree Add Variable");
            window.Source.AddVariable(variable);
        }

        private void DrawInspector()
        {
            SerializedObject serializedObject = editor.serializedObject;
            serializedObject.UpdateIfRequiredOrScript();
            SerializedProperty serializedVariables = serializedObject.FindProperty("source.sharedVariables");
            EditorGUI.BeginChangeCheck();
            if (serializedVariables.arraySize > 0)
            {
                EditorBehaviorUtility.DrawContentSeparator();
                for (int i = 0; i < serializedVariables.arraySize; i++)
                {
                    SerializedProperty serializedVariable = serializedVariables.GetArrayElementAtIndex(i);
                    SerializedProperty serializedName = serializedVariable.FindPropertyRelative("name");
                    SerializedProperty serializedValue = serializedVariable.FindPropertyRelative("value");
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(serializedValue, new GUIContent(serializedName.stringValue), true, GUILayout.Width(container.layout.width - 25f));
                    if (GUILayout.Button(EditorBehaviorUtility.LoadTexture("Icons/VariableDeleteButton"), EditorBehaviorUtility.PlainButtonGUIStyle, GUILayout.Width(18f), GUILayout.Height(18f)))
                    {
                        Undo.RecordObject(window.Behavior.Object, "BehaviorTree Delete Variable");
                        window.Source.RemoveVariable(i);
                        RefreshAddButton();
                    }

                    EditorGUILayout.EndHorizontal();
                    EditorBehaviorUtility.DrawContentSeparator();
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void RefreshAddButton()
        {
            bool canAdd = !string.IsNullOrEmpty(nameInput.value) && !window.Source.ContainsVariable(nameInput.value);
            addBtn.SetEnabled(canAdd);
        }
    }
}