using UnityEditor;
using UnityEngine;

namespace BehaviorDesigner
{
    [CustomPropertyDrawer(typeof(Vector4))]
    public class Vector4Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            property.vector4Value = EditorGUI.Vector4Field(position, label, property.vector4Value);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 38f;
        }
    }
}