using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FishRepartition))]
public class FishRepartitionEditor : Editor
{
    private enum CurveType { ClickToChoose, CONSTANT, CENTERED, SHALLOW, DEEP }

    SerializedProperty sp_init;
    SerializedProperty sp_prefab;
    SerializedProperty sp_weight;
    SerializedProperty sp_shallowest;
    SerializedProperty sp_deepest;
    SerializedProperty sp_curve;

    void OnEnable()
    {
        sp_prefab = serializedObject.FindProperty("prefab");
        sp_weight = serializedObject.FindProperty("weight");
        sp_shallowest = serializedObject.FindProperty("shallowestSpawn");
        sp_deepest = serializedObject.FindProperty("deepestSpawn");
        sp_curve = serializedObject.FindProperty("populationCurve");
    }

    public override void OnInspectorGUI()
    {
        EditorShortcuts.DrawScript_ScriptableObject<FishRepartition>(target);

        EditorGUI.BeginChangeCheck();

        // Prefab
        EditorGUILayout.PropertyField(sp_prefab);


        // Weight
        EditorGUILayout.PropertyField(sp_weight);


        // Shallowest Spawn
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.Slider(sp_shallowest, 0, 1, "Shallowest Spawn");
        if (EditorGUI.EndChangeCheck() && sp_shallowest.floatValue > sp_deepest.floatValue)
            sp_deepest.floatValue = sp_shallowest.floatValue;


        // Deepest Spawn
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.Slider(sp_deepest, 0, 1, "Deepest Spawn");
        if (EditorGUI.EndChangeCheck() && sp_deepest.floatValue < sp_shallowest.floatValue)
            sp_shallowest.floatValue = sp_deepest.floatValue;


        // Curve
        EditorGUILayout.PropertyField(sp_curve);



        // Curve Presets
        EditorGUI.indentLevel++;
        EditorGUI.BeginChangeCheck();
        CurveType curve = (CurveType)EditorGUILayout.EnumPopup(new GUIContent("Presets"), CurveType.ClickToChoose);
        EditorGUI.indentLevel--;
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


        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}