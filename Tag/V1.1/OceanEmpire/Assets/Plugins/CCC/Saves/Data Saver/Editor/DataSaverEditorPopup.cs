using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DataSaverEditorPopup : EditorWindow
{
    private DataSaverEditor.DataType dataType;
    private DataSaver gameSaves;

    private int intValue = 0;
    private bool boolValue = false;
    private float floatValue = 0;
    private string stringValue = "";
    private string keyName = "";
    private Action onAdd;

    public static void Popup(DataSaver gameSaves, DataSaverEditor.DataType dataType, Vector2 position, Action onAdd)
    {
        DataSaverEditorPopup window = ScriptableObject.CreateInstance<DataSaverEditorPopup>();
        window.position = new Rect(position.x, position.y, 250, 88);
        window.Init(gameSaves, dataType, onAdd);
        window.ShowPopup();
    }

    void Init(DataSaver gameSaves, DataSaverEditor.DataType dataType, Action onAdd)
    {
        this.gameSaves = gameSaves;
        this.dataType = dataType;
        this.onAdd = onAdd;
    }

    void OnGUI()
    {
        this.DrawWindowColor(new Color(.65f, .65f, .65f));

        EditorGUILayout.LabelField(dataType.ToString());
        EditorGUILayout.Space();

        GUILayoutOption firstBoxWidth = GUILayout.Width((position.width / 2) - 7);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Key", EditorStyles.boldLabel, firstBoxWidth);
        EditorGUILayout.LabelField("Value", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        keyName = EditorGUILayout.TextField(keyName, firstBoxWidth);

        switch (dataType)
        {
            default:
            case DataSaverEditor.DataType.Object:
            case DataSaverEditor.DataType.All:
                Debug.LogError("Error Type");
                Close();
                break;
            case DataSaverEditor.DataType.Int:
                intValue = EditorGUILayout.IntField(intValue);
                break;
            case DataSaverEditor.DataType.Bool:
                boolValue = EditorGUILayout.Toggle(boolValue);
                break;
            case DataSaverEditor.DataType.Float:
                floatValue = EditorGUILayout.FloatField(floatValue);
                break;
            case DataSaverEditor.DataType.String:
                stringValue = EditorGUILayout.TextField(stringValue);
                break;
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();


        if (GUILayout.Button("Cancel", GUILayout.Width(60)))
            Close();

        if (keyName == "")
            GUI.enabled = false;
        if (GUILayout.Button("Add"))
        {
            switch (dataType)
            {
                default:
                case DataSaverEditor.DataType.All:
                case DataSaverEditor.DataType.Object:
                    break;
                case DataSaverEditor.DataType.Int:
                    gameSaves.SetInt(keyName, intValue);
                    break;
                case DataSaverEditor.DataType.Bool:
                    gameSaves.SetBool(keyName, boolValue);
                    break;
                case DataSaverEditor.DataType.Float:
                    gameSaves.SetFloat(keyName, floatValue);
                    break;
                case DataSaverEditor.DataType.String:
                    gameSaves.SetString(keyName, stringValue);
                    break;
            }

            if (onAdd != null)
                onAdd();

            Close();
        }
        GUI.enabled = true;

        EditorGUILayout.EndHorizontal();
    }
}
