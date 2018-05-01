using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Suffix))]
public class SuffixDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Suffix suffix = attribute as Suffix;

        var suffixWidth = EditorStyles.label.CalcSize(new GUIContent(suffix.text)).x;
        position.xMax -= suffixWidth;
        EditorGUI.PropertyField(position, property, true);

        position.x = position.xMax;
        position.width = suffixWidth;

        var wasIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        EditorGUI.LabelField(position, suffix.text);
        EditorGUI.indentLevel = wasIndent;
    }
}