using UnityEditor;
using UnityEngine;

namespace BehaviorDesigner
{
    [CustomPropertyDrawer(typeof(Tag))]
    public class TagDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty serializedValue = property.FindPropertyRelative("value");
            serializedValue.stringValue = EditorGUI.TagField(position, label, serializedValue.stringValue);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}