using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(DataSaverBank))]
public class DataSaverBankEditor : Editor
{
    string[] typeNames;
    Array values;
    DataSaverBank bank;

    void OnEnable()
    {
        bank = target as DataSaverBank;
        typeNames = Enum.GetNames(typeof(DataSaverBank.Type));

        if (!bank.VerifyArrayIntegrity())
        {
            if (AssetDatabase.Contains(bank))
                EditorUtility.SetDirty(bank);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        for (int i = 0; i < typeNames.Length; i++)
        {
            DrawDataSaver(i);
        }
    }

    private void DrawDataSaver(int index)
    {
        DataSaverBank.Type type = (DataSaverBank.Type)index;
        var saver = bank.GetDataSaver(type);
        var newSaver = EditorGUILayout.ObjectField(typeNames[index], saver, typeof(DataSaver), false) as DataSaver;
        if (newSaver != saver)
        {
            bank.SetDataSaver(type, newSaver);
            EditorUtility.SetDirty(bank);
        }
    }
}
