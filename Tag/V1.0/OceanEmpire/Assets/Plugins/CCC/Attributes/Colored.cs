using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(Colored))]
public class ColoredDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Colored colored = ((Colored)attribute);

        GUI.color = colored.color;
        EditorGUI.PropertyField(position, property, true);
        GUI.color = Color.white;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}

#endif

public class Colored : PropertyAttribute
{
    public readonly Color color;

    public Colored(float r, float g, float b)
    {
        color = new Color(r, g, b);
    }
}
