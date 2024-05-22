using UnityEditor;
using UnityEngine;

namespace BehaviorDesigner
{
    [CustomPropertyDrawer(typeof(Quaternion))]
    public class QuaternionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            property.quaternionValue = Quaternion.Euler(EditorGUI.Vector3Field(position, label, property.quaternionValue.eulerAngles));
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