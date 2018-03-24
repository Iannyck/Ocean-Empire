using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapBuilder))]
public class MapBuilderEditor : Editor
{
    SerializedProperty sp_autoBuild;

    SerializedProperty sp_waterLayerContainer;
    SerializedProperty sp_waterLayerPrefab;
    SerializedProperty sp_oceanFloorInstance;

    SerializedProperty sp_height;
    SerializedProperty sp_layersSpacing;
    SerializedProperty sp_maxLateralOffset;
    SerializedProperty sp_oceanFloorOffset;
    SerializedProperty sp_waterLayersStartOffset;

    SerializedProperty sp_shallowColor;
    SerializedProperty sp_deepColor;

    void OnEnable()
    {
        sp_autoBuild = serializedObject.FindProperty("_autoBuild");

        sp_waterLayerContainer = serializedObject.FindProperty("_waterLayerContainer");
        sp_waterLayerPrefab = serializedObject.FindProperty("_waterLayerPrefab");
        sp_oceanFloorInstance = serializedObject.FindProperty("_oceanFloorInstance");

        sp_height = serializedObject.FindProperty("_depth");
        sp_layersSpacing = serializedObject.FindProperty("_layersSpacing");
        sp_maxLateralOffset = serializedObject.FindProperty("_maxLateralOffset");
        sp_oceanFloorOffset = serializedObject.FindProperty("_oceanFloorOffset");
        sp_waterLayersStartOffset = serializedObject.FindProperty("_waterLayersStartOffset");


        sp_deepColor = serializedObject.FindProperty("_deepColor");
        sp_shallowColor = serializedObject.FindProperty("_shallowColor");
    }

    public override void OnInspectorGUI()
    {
        EditorShortcuts.DrawScript_MonoBehaviour<MapBuilder>(target);

        EditorGUI.BeginChangeCheck();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(sp_autoBuild);
        if (EditorGUI.EndChangeCheck() && sp_autoBuild.boolValue)
        {
            (target as MapBuilder).RebuildWater();
        }


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("References", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(sp_waterLayerContainer);
        EditorGUILayout.PropertyField(sp_waterLayerPrefab);
        EditorGUILayout.PropertyField(sp_oceanFloorInstance);


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
        // Height
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(sp_height);
        if (EditorGUI.EndChangeCheck() && sp_autoBuild.boolValue)
        {
            (target as MapBuilder).SetDepth(sp_height.floatValue);
        }

        // Layer spacing
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(sp_layersSpacing);
        if (EditorGUI.EndChangeCheck() && sp_autoBuild.boolValue)
        {
            (target as MapBuilder).SetLayerSpacing(sp_layersSpacing.floatValue);
        }

        // Lateral offset
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(sp_maxLateralOffset);
        if (EditorGUI.EndChangeCheck() && sp_autoBuild.boolValue)
            (target as MapBuilder).SetLateralOffset(sp_maxLateralOffset.floatValue);

        // Ocean floor offset
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(sp_oceanFloorOffset);
        if (EditorGUI.EndChangeCheck() && sp_autoBuild.boolValue)
            (target as MapBuilder).SetOceanFloorOffset(sp_oceanFloorOffset.floatValue);

        // Water layers start offset
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(sp_waterLayersStartOffset);
        if (EditorGUI.EndChangeCheck() && sp_autoBuild.boolValue)
            (target as MapBuilder).SetWaterLayersStartOffset(sp_waterLayersStartOffset.floatValue);


        // Colors
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(sp_shallowColor);
        EditorGUILayout.PropertyField(sp_deepColor);
        if (EditorGUI.EndChangeCheck() && sp_autoBuild.boolValue)
        {
            (target as MapBuilder).SetColors(sp_shallowColor.colorValue, sp_deepColor.colorValue);
        }

        if (GUILayout.Button("Clear Water"))
        {
            (target as MapBuilder).ClearWater();
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}