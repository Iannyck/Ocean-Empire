
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ColoredAttribute))]
public class ColoredDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ColoredAttribute colored = ((ColoredAttribute)attribute);

        GUI.color = colored.color;
        EditorGUI.PropertyField(position, property, true);
        GUI.color = Color.white;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}