using UnityEngine;
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
