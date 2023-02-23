using System;
using UnityEditor;

namespace BehaviorDesigner.Editor
{
    public class ObjectListField : ReorderableListField
    {
        private BehaviorWindow window;

        private string ExpandedKey
        {
            get { return $"BehaviorDesign.Expanded.ObjectListField.{window.BehaviorFileId}.{Header}"; }
        }

        public ObjectListField(
            BehaviorWindow window,
            Type elementType,
            string header = "Reorder data:",
            bool isAllowReorder = true) : base(elementType, ObjectNames.NicifyVariableName(header), isAllowReorder)
        {
            this.window = window;
            IsExpended = EditorPrefs.GetBool(ExpandedKey);
            RegisterCallbacks();
        }

        private void RegisterCallbacks()
        {
            onAddItemCallback += (list, index) =>
            {
                window.RegisterUndo("List AddItem");
                list.Add(elementType.IsValueType ? Activator.CreateInstance(elementType) : null);
                window.Save();
            };

            onRemoveItemCallback += (list, index) =>
            {
                window.RegisterUndo("List RemoveItem");
                list.RemoveAt(index);
                window.Save();
            };

            onChangeItemCallback += (list, index, value) =>
            {
                window.RegisterUndo("List ChangeItem");
                list[index] = value;
                window.Save();
            };

            onReorderCallback += (list, oldIndex, newIndex) =>
            {
                window.RegisterUndo("List ChangeItem");
                object oldValue = list[oldIndex];
                list[oldIndex] = list[newIndex];
                list[newIndex] = oldValue;
                window.Save();
            };

            onExpendedCallback += isExpended =>
            {
                EditorPrefs.SetBool(ExpandedKey, isExpended);
            };
        }
    }
}