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
        DrawBonifiedTimes("Past Bonified Times", calendar.GetPastSchedules());
        DrawBonifiedTimes("Present and Future Bonified Times", calendar.GetPresentAndFutureSchedules());
    }

    private void DrawBonifiedTimes(string label, ReadOnlyCollection<Schedule> schedule)
    {
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

        EditorGUI.indentLevel++;
        if (schedule.Count == 0)
        {
            EditorGUILayout.LabelField("none", EditorStyles.centeredGreyMiniLabel);
        }
        else
        {
            for (int i = 0; i < schedule.Count; i++)
            {
                EditorGUILayout.Space();
                DrawSchedule(schedule[i]);
            }
        }
        EditorGUI.indentLevel--;
    }

    private void DrawSchedule(Schedule schedule)
    {
        EditorGUILayout.LabelField("Time Slot", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField(schedule.timeSlot.ToString());
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("Level", EditorStyles.label);
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField(schedule.task.level.ToString());
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("Min Duration", EditorStyles.label);
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField(schedule.task.minDuration.ToString());
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("Reward", EditorStyles.label);
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField(schedule.task.ticketReward + " tickets");
        EditorGUI.indentLevel--;
    }
}
