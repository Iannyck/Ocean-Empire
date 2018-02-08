using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Calendar))]
public class CalendarEditor : Editor
{
    Calendar calendar;

    private void OnEnable()
    {
        calendar = target as Calendar;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        DrawBonifiedTimes("Past Bonified Times", calendar.GetPastBonifiedTimes());
        DrawBonifiedTimes("Present and Future Bonified Times", calendar.GetPresentAndFutureBonifiedTimes());
    }

    private void DrawBonifiedTimes(string label, ReadOnlyCollection<BonifiedTime> bonifiedTimes)
    {
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

        EditorGUI.indentLevel++;
        if (bonifiedTimes.Count == 0)
        {
            EditorGUILayout.LabelField("none", EditorStyles.centeredGreyMiniLabel);
        }
        else
        {
            for (int i = 0; i < bonifiedTimes.Count; i++)
            {
                EditorGUILayout.Space();
                DrawBonifiedTime(bonifiedTimes[i]);
            }
        }
        EditorGUI.indentLevel--;
    }

    private void DrawBonifiedTime(BonifiedTime bonifiedTime)
    {
        EditorGUILayout.LabelField("Time Slot", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField(bonifiedTime.timeslot.ToString());
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("Bonus Strength", EditorStyles.label);
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField(bonifiedTime.bonusStrength.ToString());
        EditorGUI.indentLevel--;
    }
}
