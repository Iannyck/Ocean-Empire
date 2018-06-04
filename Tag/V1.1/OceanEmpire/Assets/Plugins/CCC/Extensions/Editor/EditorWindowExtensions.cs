using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorWindowExtensions
{
    public static void DrawWindowColor(this EditorWindow window, Color color)
    {
        EditorGUI.DrawRect(new Rect(Vector2.zero, window.position.size), color);
    }
}
