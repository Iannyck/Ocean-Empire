using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapBuilder))]
public class MapBuilderEditor : Editor
{
    bool autoUpdate;
    public override bool RequiresConstantRepaint()
    {
        return true;
    }

    public override void OnInspectorGUI()
    {
        if (autoUpdate)
        {
            GUI.color *= new Color(0.75f, 1, 0.75f, 1);
        }

        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);
        autoUpdate = EditorGUILayout.Toggle("Auto Update", autoUpdate);

        if (autoUpdate)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("Update") || autoUpdate)
        {
            (target as MapBuilder).UpdateAll();
        }
        GUI.enabled = true;


        if (GUILayout.Button("Clear Water"))
        {
            (target as MapBuilder).ClearWater();
        }
    }
}