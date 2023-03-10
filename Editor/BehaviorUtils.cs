using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BehaviorDesigner.Editor
{
    internal static class BehaviorUtils
    {
        public static T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public static Edge ConnectPorts(TaskPort output, TaskPort input)
        {
            Edge edge = new Edge
            {
                output = output,
                input = input
            };
            output.Connect(edge, true);
            input.Connect(edge, true);
            return edge;
        }

        public static void OpenScript(object obj)
        {
            MonoScript script = FindScript(obj);
            if (script)
            {
                AssetDatabase.OpenAsset(script);
            }
        }

        public static void SelectScript(object obj)
        {
            MonoScript script = FindScript(obj);
            if (script)
            {
                Selection.activeObject = script;
            }
        }

        public static MonoScript FindScript(object obj)
        {
            MonoScript[] scripts = Resources.FindObjectsOfTypeAll<MonoScript>();
            foreach (MonoScript script in scripts)
            {
                if (script && script.GetClass() == obj.GetType())
                {
                    return script;
                }
            }

            return null;
        }

        public static long GetFileId(Object obj)
        {
            PropertyInfo info = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);
            SerializedObject serializedObj = new SerializedObject(obj);
            info.SetValue(serializedObj, InspectorMode.Debug, null);
            SerializedProperty fileId = serializedObj.FindProperty("m_LocalIdentfierInFile");
            return fileId.longValue;
        }

        public static bool HasComponent(GameObject go, int instanceID)
        {
            return EditorUtility.InstanceIDToObject(instanceID) is Component component && component.gameObject == go;
        }
    }
}