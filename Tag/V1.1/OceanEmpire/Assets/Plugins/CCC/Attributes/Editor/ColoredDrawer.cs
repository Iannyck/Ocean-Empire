
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ColoredAttribute))]
public class ColoredDrawer : PropertyDrawer
{
    //public override void OnGUI(Rect position)
    //{
    //    ColoredAttribute colored = ((ColoredAttribute)attribute);
    //    GUI.color = colored.color;
    //    //base.OnGUI(position);
    //}
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ColoredAttribute colored = ((ColoredAttribute)attribute);

        EditorGUI.PropertyField(position, property, true);
        GUI.color = Color.white;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}