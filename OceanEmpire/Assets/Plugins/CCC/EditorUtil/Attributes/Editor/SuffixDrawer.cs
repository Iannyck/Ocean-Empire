using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Suffix))]
public class SuffixDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Suffix suffix = attribute as Suffix;

        var suffixWidth = GUILayoutUtility.GetRect(new GUIContent(suffix.text), EditorStyles.label, GUILayout.ExpandWidth(false)).width;

        position.xMax -= suffixWidth;
        EditorGUI.PropertyField(position, property, true);

        position.x += position.width;
        position.width = suffixWidth;
        EditorGUI.LabelField(position, suffix.text);
    }
}