using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyInPlayMode))]
public class ReadOnlyInPlayModeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, (attribute as ReadOnlyInPlayMode).forwardToChildren);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (Application.isPlaying)
            GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, (attribute as ReadOnlyInPlayMode).forwardToChildren);
        GUI.enabled = true;
    }
}

#endif

public class ReadOnlyInPlayMode : PropertyAttribute
{
    public bool forwardToChildren = true;
    public ReadOnlyInPlayMode() { }
}
