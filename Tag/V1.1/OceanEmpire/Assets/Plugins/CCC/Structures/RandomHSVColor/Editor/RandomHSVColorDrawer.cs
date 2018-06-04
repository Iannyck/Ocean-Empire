using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RandomHSVColor))]
public class RandomHSVColorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position = EditorGUI.IndentedRect(position);


        EditorGUI.indentLevel = 0;
        EditorGUI.BeginProperty(position, label, property);

        var labelWidth = EditorStyles.label.CalcSize(label).x + 8;

        Rect labelRect = new Rect(position)
        {
            width = labelWidth
        };
        EditorGUI.LabelField(labelRect, label, EditorStyles.label);

        Rect miniRect = new Rect(position.x + labelWidth, position.y, (position.width - labelWidth) / 2, position.height);

        SerializedProperty left = property.FindPropertyRelative("left");
        SerializedProperty right = property.FindPropertyRelative("right");

        DrawColor(miniRect, left, "left:");
        miniRect.x += miniRect.width;
        DrawColor(miniRect, right, "right:");

        EditorGUI.EndProperty();
    }

    private void DrawColor(Rect rect, SerializedProperty property, string label)
    {
        var labelContent = new GUIContent(label);
        var labelWidth = EditorStyles.label.CalcSize(labelContent).x;
        Rect labelRect = new Rect(rect)
        {
            width = labelWidth
        };
        EditorGUI.LabelField(labelRect, labelContent, EditorStyles.label);
        rect.width -= labelWidth;
        rect.x += labelWidth;
        EditorGUI.PropertyField(rect, property, GUIContent.none);
    }
}
