using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class VariableField : VisualElement
    {
        private VariablesView view;
        private SharedVariable variable;
        private VisualElement field;
        private Foldout foldout;
        private TextField nameInput;
        private DropdownField typeDp;
        private Button upBtn;
        private Button downBtn;
        private Button deleteBtn;

        public SharedVariable Value
        {
            get { return variable; }
        }

        public VariableField(VariablesView view, SharedVariable variable, VisualElement field)
        {
            this.view = view;
            this.variable = variable;
            this.field = field;
            BehaviorUtils.Load<VisualTreeAsset>("UXML/VariableField").CloneTree(this);
            styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/VariablesView"));
            foldout = this.Q<Foldout>();
            foldout.text = variable.Name;
            foldout.Add(field);
            string key = $"BehaviorDesign.Expanded.{view.Window.BehaviorFileId}.{variable.Name}";
            foldout.value = EditorPrefs.GetBool(key, false);
            foldout.RegisterValueChangedCallback(evt =>
            {
                EditorPrefs.SetBool(key, evt.newValue);
            });

            nameInput = this.Q<TextField>("name-input");
            nameInput.value = variable.Name;

            typeDp = this.Q<DropdownField>("type-dp");
            typeDp.choices = view.VariableTypeChoices;
            typeDp.SetValueWithoutNotify(variable.GetType().Name.Replace("Shared", ""));

            upBtn = this.Q<Button>("up-btn");
            downBtn = this.Q<Button>("down-btn");
            deleteBtn = this.Q<Button>("delete-btn");
        }

        public void RegisterCallback(
            Func<string, VariableField, bool> onNameChanged,
            Action<int, VariableField> onTypeChanged,
            Action<VariableField, bool> onMoveClicked,
            Action<VariableField> onDeleteClicked)
        {
            nameInput.RegisterCallback<FocusOutEvent>(evt =>
            {
                if (!onNameChanged(nameInput.value, this))
                {
                    nameInput.SetValueWithoutNotify(variable.Name);
                }

                foldout.text = variable.Name;
            });

            typeDp.RegisterValueChangedCallback(evt =>
            {
                onTypeChanged(typeDp.index, this);
            });
            
            upBtn.clickable.clicked += () =>
            {
                onMoveClicked(this, false);
            };
            
            downBtn.clickable.clicked += () =>
            {
                onMoveClicked(this, true);
            };
            
            deleteBtn.clickable.clicked += () =>
            {
                onDeleteClicked(this);
            };
        }

        public void UpdateBtnState()
        {
            if (!view.Window.Source.CanMoveVariable(variable.Name, false))
            {
                upBtn.SetEnabled(false);
            }
            else
            {
                upBtn.SetEnabled(true);
            }

            if (!view.Window.Source.CanMoveVariable(variable.Name, true))
            {
                downBtn.SetEnabled(false);
            }
            else
            {
                downBtn.SetEnabled(true);
            }
        }
    }
}