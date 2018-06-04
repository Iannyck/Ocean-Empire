using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SharedTables))]
public class SharedTablesEditor : Editor
{
    bool resetNextLayout = false;

    public override bool RequiresConstantRepaint()
    {
        return true;
    }

    public override void OnInspectorGUI()
    {
        SharedTables ss = target as SharedTables;

        bool wasRemoveEmptySeats = ss.RemoveEmptySeats;
        int wasMaxSeats = ss.MaxSeatsPerTable;

        base.OnInspectorGUI();

        //Pour s'assurer que ça "apply" sur les tables
        if (ss.RemoveEmptySeats != wasRemoveEmptySeats)
        {
            ss.RemoveEmptySeats = ss.RemoveEmptySeats;
        }
        if (ss.MaxSeatsPerTable != wasMaxSeats)
        {
            ss.MaxSeatsPerTable = ss.MaxSeatsPerTable;
        }

        if (!EditorApplication.isPlaying)
        {
            if (ss.GetTableCount() != ss.GetStartingTableCount() || (Event.current.type == EventType.Layout && resetNextLayout))
            {
                resetNextLayout = false;
                ss.Reset();
            }
        }


        EditorGUILayout.Space();

        GUI.enabled = false;
        List<SharedTables.Table> tables = ss.GetTables();
        int count = tables.Count;
        for (int i = 0; i < count; i++)
        {
            EditorGUILayout.LabelField("Table " + i, EditorStyles.boldLabel);

            if (tables[i].SeatCount == 0)
            {
                EditorGUILayout.LabelField("     No seats", EditorStyles.miniLabel);
            }
            else
            {
                if (!EditorApplication.isPlaying)
                    resetNextLayout = true;

                for (int j = 0; j < tables[i].SeatCount; j++)
                {
                    object obj = tables[i].seats[j];
                    string name;
                    if (obj == null)
                    {
                        name = "Null";
                    }
                    else
                    {
                        if (obj is Object)
                        {
                            Object unityObj = (Object)obj;
                            name = unityObj == null ? "Null" : unityObj.name;
                        }
                        else
                        {
                            name = obj.ToString();
                        }
                    }
                    EditorGUILayout.LabelField("     Seat " + j + ":  " + name);
                }
            }
        }
        GUI.enabled = true;
    }
}