using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class ReorderableListField : BaseField<IList>
    {
        protected IList dataList;
        protected Type elementType;
        private ReorderableList reorderableList;
        private IMGUIContainer container;
        private GUIStyle labelStyle;
        private string header;
        private bool isAllowReorder;
        private bool isExpanded;

        public Action<IList, int> onAddItemCallback;
        public Action<IList, int> onRemoveItemCallback;
        public Action<IList, int, object> onChangeItemCallback;
        public Action<IList, int, int> onReorderCallback;
        public Action<bool> onExpendedCallback;

        private static MethodInfo doListHeader;

        public string Header
        {
            get { return header; }
            set { header = value; }
        }
        
        public bool IsExpended
        {
            get { return isExpanded; }
            set { isExpanded = value; }
        }

        public override IList value
        {
            get { return dataList; }
            set { RecreateList(value); }
        }

        public ReorderableListField(
            Type elementType,
            string header = "Reorder data:",
            bool isAllowReorder = true) : base(null, null)
        {
            Clear();
            this.elementType = elementType;
            this.header = header;
            this.isAllowReorder = isAllowReorder;
            
            styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/ReorderableList"));
            container = new IMGUIContainer(OnGUIHandler)
            {
                name = "ListContainer",
                style =
                {
                    flexShrink = 1,
                    flexGrow = 1
                }
            };

            if (elementType == typeof(Vector2) ||
                elementType == typeof(Vector2Int))
            {
                AddToClassList("list-v2");
            }
            else if (elementType == typeof(Vector3) ||
                     elementType == typeof(Vector3Int))
            {
                AddToClassList("list-v3");
            }
            else if (elementType == typeof(Vector4) ||
                     elementType == typeof(Quaternion))
            {
                AddToClassList("list-v4");
            }
            else
            {
                AddToClassList("list-v1");
            }

            Add(container);

            if (doListHeader == null)
            {
                doListHeader = typeof(ReorderableList).GetMethod("DoListHeader", BindingFlags.Instance | BindingFlags.NonPublic);
            }
        }

        private void RecreateList(IList dataList)
        {
            if (dataList == null)
            {
                Type listType = typeof(List<>).MakeGenericType(elementType);
                dataList = (IList)Activator.CreateInstance(listType);
            }

            this.dataList = dataList;
            reorderableList = new ReorderableList(
                dataList,
                elementType,
                isAllowReorder,
                false,
                true,
                true
            );

            AddCallbacks();
        }

        private void OnGUIHandler()
        {
            try
            {
                if (DrawHeader())
                {
                    reorderableList?.DoLayoutList();
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private bool DrawHeader()
        {
            EditorGUI.BeginChangeCheck();
            Rect rect = GUILayoutUtility.GetRect(0.0f, 20f, GUILayout.ExpandWidth(true));
            if (GUI.Button(rect, ""))
            {
                isExpanded = !isExpanded;
            }

            ReorderableList.defaultBehaviours.DrawHeaderBackground(rect);
            isExpanded = EditorGUI.Foldout(rect, isExpanded, header);
            if (EditorGUI.EndChangeCheck())
            {
                onExpendedCallback?.Invoke(isExpanded);
            }

            return isExpanded;
        }

        private void AddCallbacks()
        {
            reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                Rect labelRect = new Rect(rect.x, rect.y, 80f, EditorGUIUtility.singleLineHeight);
                Rect fieldRect = new Rect(rect.x + 80f, rect.y, rect.width - 80f, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(labelRect, $"Element {index}");
                object value = dataList[index];

                EditorGUI.BeginChangeCheck();
                value = DrawObject(fieldRect, value);
                if (EditorGUI.EndChangeCheck())
                {
                    if (onChangeItemCallback != null)
                    {
                        onChangeItemCallback?.Invoke(dataList, index, value);
                    }
                    else
                    {
                        dataList[index] = value;
                    }
                }
            };

            reorderableList.onReorderCallbackWithDetails += OnReorderItems;
            reorderableList.onAddCallback += OnAddItem;
            reorderableList.onRemoveCallback += OnRemoveItem;
        }

        private void OnReorderItems(ReorderableList list, int oldIndex, int newIndex)
        {
            if (onReorderCallback != null)
            {
                object oldValue = dataList[oldIndex];
                dataList[oldIndex] = dataList[newIndex];
                dataList[newIndex] = oldValue;
                onReorderCallback?.Invoke(dataList, oldIndex, newIndex);
            }
        }

        private void OnAddItem(ReorderableList list)
        {
            if (onAddItemCallback != null)
            {
                onAddItemCallback(dataList, dataList.Count);
            }
            else
            {
                dataList.Add(elementType.IsValueType ? Activator.CreateInstance(elementType) : null);
            }
        }

        private void OnRemoveItem(ReorderableList list)
        {
            int indexToRemove = list.index;
            if (indexToRemove < 0)
            {
                indexToRemove = dataList.Count - 1;
            }

            if (indexToRemove >= 0)
            {
                if (onRemoveItemCallback != null)
                {
                    onRemoveItemCallback(dataList, indexToRemove);
                }
                else
                {
                    dataList.RemoveAt(indexToRemove);
                }
            }
        }

        private object DrawObject(Rect rect, object value)
        {
            try
            {
                if (elementType == typeof(string))
                {
                    value = EditorGUI.TextField(rect, value == null ? "" : value.ToString());
                }
                else if (elementType == typeof(bool))
                {
                    value = EditorGUI.Toggle(rect, (bool) value);
                }
                else if (elementType == typeof(int))
                {
                    value = EditorGUI.IntField(rect, (int) value);
                }
                else if (elementType == typeof(long))
                {
                    value = EditorGUI.LongField(rect, (long) value);
                }
                else if (elementType == typeof(float))
                {
                    value = EditorGUI.FloatField(rect, (float) value);
                }
                else if (elementType == typeof(double))
                {
                    value = EditorGUI.DoubleField(rect, (double) value);
                }
                else if (elementType == typeof(Vector2))
                {
                    value = EditorGUI.Vector2Field(rect, "", (Vector2) value);
                }
                else if (elementType == typeof(Vector2Int))
                {
                    value = EditorGUI.Vector2IntField(rect, "", (Vector2Int) value);
                }
                else if (elementType == typeof(Vector3))
                {
                    value = EditorGUI.Vector3Field(rect, "", (Vector3) value);
                }
                else if (elementType == typeof(Vector3Int))
                {
                    value = EditorGUI.Vector3IntField(rect, "", (Vector3Int) value);
                }
                else if (elementType == typeof(Vector4))
                {
                    value = EditorGUI.Vector4Field(rect, "", (Vector4) value);
                }
                else if (elementType == typeof(Quaternion))
                {
                    Quaternion quaternion = (Quaternion) value;
                    Vector4 zero = Vector4.zero;
                    zero.Set(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
                    Vector4 vector4 = EditorGUI.Vector4Field(rect, "", zero);
                    quaternion.Set(vector4.x, vector4.y, vector4.z, vector4.w);
                    value = quaternion;
                }
                else if (elementType == typeof(AnimationCurve))
                {
                    value = EditorGUI.CurveField(rect, (AnimationCurve) value);
                }
                else if (elementType == typeof(Color))
                {
                    value = EditorGUI.ColorField(rect, (Color) value);
                }
                else if (elementType == typeof(Rect))
                {
                    value = EditorGUI.RectField(rect, (Rect) value);
                }
                else if (elementType == typeof(RectInt))
                {
                    value = EditorGUI.RectIntField(rect, (RectInt) value);
                }
                else
                {
                    value = EditorGUI.ObjectField(rect, (UnityEngine.Object) value, elementType, true);
                }
            }
            catch
            {
            }

            return value;
        }
    }
}