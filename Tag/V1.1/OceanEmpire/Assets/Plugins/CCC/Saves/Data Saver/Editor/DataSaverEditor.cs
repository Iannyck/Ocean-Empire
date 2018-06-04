using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataSaver))]
public class DataSaverEditor : Editor
{

    public enum DataType { All, Int, Bool, Float, String, Object }
    private const int dataTypeCount = 6;

    DataSaver gameSaves;
    DataType chosenDataType = DataType.All;

    string[] keys;
    int[] keyTypeCounts = new int[dataTypeCount]; // Utilisé pour dessiner le data type All

    bool loadCategory = false;
    bool clearCategory = false;

    void OnEnable()
    {
        gameSaves = target as DataSaver;

        RefreshKeys();
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Save Location"))
        {
            string path = Application.persistentDataPath.Replace('/', '\\');

            if (Directory.Exists(path))
            {
                System.Diagnostics.Process.Start("explorer.exe", path);
            }
        }


        base.OnInspectorGUI();

        if (Event.current.type == EventType.Layout)
            ExecuteAwaitingActions();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Types", EditorStyles.boldLabel);
        DrawDataTypeButtons();

        EditorGUILayout.Space();

        DrawRefreshButton();
        DrawData();

        EditorGUILayout.LabelField("File Operations", EditorStyles.boldLabel);
        DrawUtilityButtons();

        EditorGUILayout.Space();
    }

    private void RefreshKeys()
    {
        switch (chosenDataType)
        {
            case DataType.All:
                {
                    var intKeys = gameSaves.GetIntKeys();
                    var boolKeys = gameSaves.GetBoolKeys();
                    var stringKeys = gameSaves.GetStringKeys();
                    var floatKeys = gameSaves.GetFloatKeys();
                    var objectKeys = gameSaves.GetObjectKeys();

                    keys = new string[intKeys.Count + boolKeys.Count + floatKeys.Count + stringKeys.Count + objectKeys.Count];

                    var current = 0;

                    //All
                    keyTypeCounts[0] = 0;

                    //Ints
                    keyTypeCounts[1] = current;
                    intKeys.CopyTo(keys, current);
                    current += intKeys.Count;

                    //Bools
                    keyTypeCounts[2] = current;
                    boolKeys.CopyTo(keys, current);
                    current += boolKeys.Count;

                    //Floats
                    keyTypeCounts[3] = current;
                    floatKeys.CopyTo(keys, current);
                    current += floatKeys.Count;

                    //Strings
                    keyTypeCounts[4] = current;
                    stringKeys.CopyTo(keys, current);
                    current += stringKeys.Count;

                    //Objects
                    keyTypeCounts[5] = current;
                    objectKeys.CopyTo(keys, current);
                    break;
                }
            case DataType.Int:
                {
                    var newKeys = gameSaves.GetIntKeys();
                    keys = new string[newKeys.Count];
                    newKeys.CopyTo(keys, 0);
                    break;
                }
            case DataType.Float:
                {
                    var newKeys = gameSaves.GetFloatKeys();
                    keys = new string[newKeys.Count];
                    newKeys.CopyTo(keys, 0);
                    break;
                }
            case DataType.String:
                {
                    var newKeys = gameSaves.GetStringKeys();
                    keys = new string[newKeys.Count];
                    newKeys.CopyTo(keys, 0);
                    break;
                }
            case DataType.Object:
                {
                    var newKeys = gameSaves.GetObjectKeys();
                    keys = new string[newKeys.Count];
                    newKeys.CopyTo(keys, 0);
                    break;
                }
            case DataType.Bool:
                {
                    var newKeys = gameSaves.GetBoolKeys();
                    keys = new string[newKeys.Count];
                    newKeys.CopyTo(keys, 0);
                    break;
                }
            default:
                break;
        }
    }

    private static Color StandardToSelectedColor(Color normalColor)
    {
        return new Color(normalColor.r * 0.25f, normalColor.g * 1, normalColor.b * 0.35f, normalColor.a);
    }
    private static Color StandardToRefreshColor(Color normalColor)
    {
        return new Color(normalColor.r * 0.8f, normalColor.g * .87f, normalColor.b * 1, normalColor.a);
    }

    private void DrawDataTypeButtons()
    {
        var stdColor = GUI.color;
        var selectedColor = StandardToSelectedColor(stdColor);

        bool refreshKeys = false;

        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < dataTypeCount; i++)
        {
            var dataType = (DataType)i;
            GUI.color = dataType == chosenDataType ? selectedColor : stdColor;

            if (GUILayout.Button(dataType.ToString()))
            {
                chosenDataType = dataType;
                refreshKeys = true;
            }
        }
        EditorGUILayout.EndHorizontal();

        GUI.color = stdColor;

        if (refreshKeys)
            RefreshKeys();

    }

    private void DrawRefreshButton()
    {
        var guiColor = GUI.color;
        GUI.color = StandardToRefreshColor(guiColor);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Refresh"))
        {
            RefreshKeys();
        }
        EditorGUILayout.EndHorizontal();

        GUI.color = guiColor;
    }

    private void DrawData()
    {
        OneWayBool refresh = new OneWayBool(false);

        if (chosenDataType == DataType.All)
        {
            if (keys == null)
            {
                for (int i = 1; i < dataTypeCount; i++)
                {
                    var dataType = (DataType)i;
                    refresh.TryToSet(!DrawData(dataType, -1, -1));
                }
            }
            else
            {
                for (int i = 1; i < dataTypeCount; i++)
                {
                    var dataType = (DataType)i;
                    var begin = keyTypeCounts[i];
                    var end = (i + 1) >= dataTypeCount ? keys.Length : keyTypeCounts[i + 1];

                    refresh.TryToSet(!DrawData(dataType, begin, end));
                }
            }
        }
        else
        {
            refresh.TryToSet(!DrawData(chosenDataType, 0, keys != null ? keys.Length : 0));
        }

        if (refresh)
            RefreshKeys();
    }

    /// <summary>
    /// Retourne Faux si une key n'existait pas
    /// </summary>
    private bool DrawData(DataType type, int keysStart, int keysEnd)
    {
        OneWayBool allKeyExist = new OneWayBool(true);

        EditorGUILayout.BeginHorizontal();


        // + Button
        if (IsDataTypeModifiable(type))
        {
            var p = GUI.skin.button.padding;
            var o = GUI.skin.button.contentOffset;
            RectOffset wasPadding = new RectOffset(p.left, p.right, p.top, p.bottom);
            Vector2 wasContentOffset = new Vector2(o.x, o.y);

            GUI.skin.button.padding = new RectOffset(0, 0, 0, 0);
            GUI.skin.button.contentOffset = new Vector2(0, -1);

            if (GUILayout.Button("+", GUILayout.Width(16), GUILayout.Height(16)))
            {
                var screenPoint = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                DataSaverEditorPopup.Popup(gameSaves, type, screenPoint, RefreshKeys);
            }
            GUI.skin.button.padding = wasPadding;
            GUI.skin.button.contentOffset = wasContentOffset;
        }

        // Label
        EditorGUILayout.LabelField(type.ToString(), EditorStyles.boldLabel);

        EditorGUILayout.EndHorizontal();


        // Data
        if (keysStart == keysEnd) // If length == 0
        {
            EditorGUILayout.LabelField("Empty", EditorStyles.centeredGreyMiniLabel);
        }
        else
        {
            for (int i = keysStart; i < keysEnd; i++)
            {
                allKeyExist.TryToSet(DrawData(type, keys[i]));
            }
        }
        EditorGUILayout.Space();
        return allKeyExist;
    }

    private static bool IsDataTypeModifiable(DataType type)
    {
        switch (type)
        {
            default:
            case DataType.Object:
            case DataType.All:
                return false;
            case DataType.Int:
            case DataType.Bool:
            case DataType.Float:
            case DataType.String:
                return true;
        }
    }

    /// <summary>
    /// Retourne Faux si la key n'existait pas
    /// </summary>
    private bool DrawData(DataType type, string key)
    {
        EditorGUILayout.BeginHorizontal();

        var deleteKey = GUILayout.Button("X", GUILayout.Height(16), GUILayout.Width(16));

        bool keyExists = true;
        switch (type)
        {
            case DataType.Int:
                {
                    if (deleteKey)
                        gameSaves.DeleteInt(key);

                    EditorGUI.BeginChangeCheck();
                    var newValue = EditorGUILayout.DelayedIntField(key, gameSaves.GetInt(key));
                    keyExists = gameSaves.ContainsInt(key);

                    if (EditorGUI.EndChangeCheck())
                        gameSaves.SetInt(key, newValue);

                    break;
                }

            case DataType.Float:
                {
                    if (deleteKey)
                        gameSaves.DeleteFloat(key);

                    EditorGUI.BeginChangeCheck();
                    var newValue = EditorGUILayout.DelayedFloatField(key, gameSaves.GetFloat(key));
                    keyExists = gameSaves.ContainsFloat(key);

                    if (EditorGUI.EndChangeCheck())
                        gameSaves.SetFloat(key, newValue);
                    break;
                }

            case DataType.String:
                {
                    if (deleteKey)
                        gameSaves.DeleteString(key);

                    EditorGUI.BeginChangeCheck();
                    var newValue = EditorGUILayout.DelayedTextField(key, gameSaves.GetString(key));
                    keyExists = gameSaves.ContainsString(key);

                    if (EditorGUI.EndChangeCheck())
                        gameSaves.SetString(key, newValue);
                    break;
                }

            case DataType.Bool:
                {
                    if (deleteKey)
                        gameSaves.DeleteBool(key);

                    EditorGUI.BeginChangeCheck();
                    var newValue = EditorGUILayout.Toggle(key, gameSaves.GetBool(key));
                    keyExists = gameSaves.ContainsBool(key);

                    if (EditorGUI.EndChangeCheck())
                        gameSaves.SetBool(key, newValue);
                    break;
                }

            case DataType.Object:
                {
                    if (deleteKey)
                        gameSaves.DeleteObjectClone(key);

                    var obj = gameSaves.GetObjectClone(key);
                    var text = key + ": " + (obj == null ? "null" : obj.GetType().ToString());
                    EditorGUILayout.LabelField(text);
                    keyExists = gameSaves.ContainsObject(key);

                    break;
                }
            default:
                EditorGUILayout.LabelField("Error type", EditorStyles.whiteBoldLabel);
                break;
        }

        EditorGUILayout.EndHorizontal();

        return keyExists;
    }

    private void DrawUtilityButtons()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            gameSaves.Save();
        }

        if (GUILayout.Button("Load"))
        {
            loadCategory = true;
        }

        if (GUILayout.Button("Clear Save"))
        {
            clearCategory = true;
        }
        EditorGUILayout.EndHorizontal();
    }

    private void ExecuteAwaitingActions()
    {
        if (clearCategory)
        {
            gameSaves.ClearSave();
            clearCategory = false;
            RefreshKeys();
        }
        if (loadCategory)
        {
            gameSaves.Load();
            loadCategory = false;
            RefreshKeys();
        }
    }
}
