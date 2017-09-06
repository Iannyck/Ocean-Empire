using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnly))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, (attribute as ReadOnly).forwardToChildren);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, (attribute as ReadOnly).forwardToChildren);
        GUI.enabled = true;
    }
}

#endif

public class ReadOnly : PropertyAttribute
{
    public bool forwardToChildren = true;
    public ReadOnly() { }
}
