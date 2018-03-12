using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FishRepartition))]
public class FishRepartitionDrawer : PropertyDrawer
{
    private enum CurveType { ClickToChoose, CONSTANT, CENTERED, SHALLOW, DEEP }

    private const float SPACING = 20;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        //position = EditorGUI.IndentedRect(position);
        position.height = 18;


        //EditorGUI.indentLevel = 0;
        EditorGUI.BeginProperty(position, label, property);

        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
        position.y += SPACING;

        if (property.isExpanded)
        {
            SerializedProperty sp_init = property.FindPropertyRelative("hasBeenInitialized");
            SerializedProperty sp_prefab = property.FindPropertyRelative("prefab");
            SerializedProperty sp_weight = property.FindPropertyRelative("weight");
            SerializedProperty sp_shallowest = property.FindPropertyRelative("shallowestSpawn");
            SerializedProperty sp_deepest = property.FindPropertyRelative("deepestSpawn");
            SerializedProperty sp_curve = property.FindPropertyRelative("populationCurve");

            if (!sp_init.boolValue)
            {
                sp_init.boolValue = true;
                sp_weight.floatValue = 1;
                sp_shallowest.floatValue = 0;
                sp_deepest.floatValue = 1;
                sp_curve.animationCurveValue = FishRepartition.CURVE_CONSTANT;
            }

            position = EditorGUI.IndentedRect(position);

            // Prefab
            EditorGUI.PropertyField(position, sp_prefab);
            position.y += SPACING;

            
            // Weight
            EditorGUI.PropertyField(position, sp_weight);
            position.y += SPACING;


            // Shallowest Spawn
            EditorGUI.BeginChangeCheck();
            EditorGUI.Slider(position, sp_shallowest, 0, 1, "Shallowest Spawn");
            position.y += SPACING;
            if (EditorGUI.EndChangeCheck() && sp_shallowest.floatValue > sp_deepest.floatValue)
                sp_deepest.floatValue = sp_shallowest.floatValue;


            // Deepest Spawn
            EditorGUI.BeginChangeCheck();
            EditorGUI.Slider(position, sp_deepest, 0, 1, "Deepest Spawn");
            position.y += SPACING;
            if (EditorGUI.EndChangeCheck() && sp_deepest.floatValue < sp_shallowest.floatValue)
                sp_shallowest.floatValue = sp_deepest.floatValue;

            
            // Curve
            EditorGUI.PropertyField(position, sp_curve);
            position.y += SPACING;



            // Curve Presets
            position = EditorGUI.IndentedRect(position);
            EditorGUI.BeginChangeCheck();
            CurveType curve = (CurveType)EditorGUI.EnumPopup(position, new GUIContent("Presets"), CurveType.ClickToChoose);
            if (EditorGUI.EndChangeCheck())
            {
                switch (curve)
                {
                    case CurveType.CONSTANT:
                        sp_curve.animationCurveValue = FishRepartition.CURVE_CONSTANT;
                        break;
                    case CurveType.CENTERED:
                        sp_curve.animationCurveValue = FishRepartition.CURVE_CENTERED;
                        break;
                    case CurveType.SHALLOW:
                        sp_curve.animationCurveValue = FishRepartition.CURVE_SHALLOW;
                        break;
                    case CurveType.DEEP:
                        sp_curve.animationCurveValue = FishRepartition.CURVE_DEEP;
                        break;
                }
            }
            position.y += SPACING;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return property.isExpanded ? SPACING * 7 : SPACING;
    }
}