using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EditorShortcuts
{
    public static void DrawScript_ScriptableObject<T>(Object target) where T: ScriptableObject
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((T)target), typeof(T), false);
        GUI.enabled = true;
    }

    public static void DrawScript_MonoBehaviour<T>(Object target) where T : MonoBehaviour
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((T)target), typeof(T), false);
        GUI.enabled = true;
    }
}
