using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BehaviorDesigner
{
    internal static class EditorBehaviorUtility
    {
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private static GUIStyle plainButtonGUIStyle;

        public static GUIStyle PlainButtonGUIStyle
        {
            get
            {
                if (plainButtonGUIStyle == null)
                {
                    plainButtonGUIStyle = new GUIStyle(GUI.skin.button)
                    {
                        margin = new RectOffset(0, 0, 2, 2),
                        padding = new RectOffset(0, 0, 1, 0)
                    };
                }

                return plainButtonGUIStyle;
            }
        }

        public static void DrawContentSeparator()
        {
            EditorGUILayout.Space(2f);
            Rect lastRect = GUILayoutUtility.GetLastRect();
            lastRect.y += lastRect.height;
            lastRect.height = 2f;
            GUI.DrawTexture(lastRect, LoadTexture("Icons/ContentSeparator"));
            EditorGUILayout.Space(2f);
        }

        public static Texture2D LoadTexture(string path)
        {
            if (!textures.TryGetValue(path, out Texture2D texture))
            {
                texture = Load<Texture2D>(path);
                if (texture == null)
                {
                    return null;
                }

                textures.Add(path, texture);
            }

            return texture;
        }

        public static T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public static object Copy(object obj, bool deep)
        {
            if (obj == null || obj is string || obj.GetType().IsValueType || obj is Object)
            {
                return obj;
            }

            Type type = obj.GetType();
            if (type.IsArray)
            {
                Array array = Array.CreateInstance(type.GetElementType(), ((Array) obj).Length);
                for (int i = 0; i < ((Array) obj).Length; i++)
                {
                    array.SetValue(Copy(((Array) obj).GetValue(i), true), i);
                }

                return array;
            }

            object instance = Activator.CreateInstance(type);

            if (!deep)
            {
                return instance;
            }

            while (type.BaseType != null)
            {
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    try
                    {
                        if (field.IsDefined(typeof(SerializeReference)))
                        {
                            field.SetValue(instance, Copy(field.GetValue(obj), false));
                        }
                        else if (field.IsPublic || field.IsDefined(typeof(SerializeField)) || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            field.SetValue(instance, Copy(field.GetValue(obj), true));
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }

                type = type.BaseType;
            }

            return instance;
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

        public static bool TryGetGuid(Object obj, out string guid)
        {
            return AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out guid, out long fileId);
        }

        public static Vector3 GetPrefsVector3(string key, Vector3 defaultValue)
        {
            defaultValue.x = EditorPrefs.GetFloat($"{key}_x", defaultValue.x);
            defaultValue.y = EditorPrefs.GetFloat($"{key}_y", defaultValue.y);
            defaultValue.z = EditorPrefs.GetFloat($"{key}_z", defaultValue.z);
            return defaultValue;
        }

        public static void SetPrefsVector3(string key, Vector3 value)
        {
            EditorPrefs.SetFloat($"{key}_x", value.x);
            EditorPrefs.SetFloat($"{key}_y", value.y);
            EditorPrefs.SetFloat($"{key}_z", value.z);
        }
    }
}