using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
class MinMaxSliderDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        if (property.propertyType == SerializedPropertyType.Vector2)
        {
            Vector2 range = property.vector2Value;
            float min = range.x;
            float max = range.y;
            MinMaxSliderAttribute attr = attribute as MinMaxSliderAttribute;

            const float dataWidth = 60;
            const float spacing = 5;

            Rect leftHandSide = position;
            leftHandSide.max = leftHandSide.max - Vector2.right * (dataWidth * 2 + spacing * 2);

            EditorGUI.BeginChangeCheck();

            EditorGUI.MinMaxSlider(leftHandSide, label, ref min, ref max, attr.min, attr.max);

            if (EditorGUI.EndChangeCheck())
            {
                range.x = min;
                range.y = max;
                property.vector2Value = range;
            }


            Rect minRegion = new Rect(leftHandSide.xMax + spacing, leftHandSide.y, dataWidth, leftHandSide.height);
            Rect maxRegion = new Rect(minRegion.xMax + spacing, leftHandSide.y, dataWidth, leftHandSide.height);

            EditorGUI.BeginChangeCheck();

            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 0;
            min = EditorGUI.FloatField(minRegion, min);
            max = EditorGUI.FloatField(maxRegion, max);
            EditorGUIUtility.labelWidth = labelWidth;
            if (EditorGUI.EndChangeCheck())
            {
                range.x = min;
                range.y = max;
                property.vector2Value = range;
            }
        }
        else
        {
            EditorGUI.LabelField(position, label, "Use only with Vector2");
        }
    }
}
