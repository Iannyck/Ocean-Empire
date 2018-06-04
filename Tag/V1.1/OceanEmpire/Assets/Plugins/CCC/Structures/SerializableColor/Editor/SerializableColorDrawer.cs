using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SerializableColor))]
public class SerializableColorDrawer : BitMaskPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var r = property.FindPropertyRelative("r");
        var g = property.FindPropertyRelative("g");
        var b = property.FindPropertyRelative("b");
        var a = property.FindPropertyRelative("a");

        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();

        var newColor = EditorGUI.ColorField(position, label, new Color(r.floatValue, g.floatValue, b.floatValue, a.floatValue));
        if (EditorGUI.EndChangeCheck())
        {
            r.floatValue = newColor.r;
            g.floatValue = newColor.g;
            b.floatValue = newColor.b;
            a.floatValue = newColor.a;
        }

        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 16;
    }
}
