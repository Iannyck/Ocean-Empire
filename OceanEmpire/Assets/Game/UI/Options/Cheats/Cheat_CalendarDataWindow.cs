using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using UnityEngine.UI;

public class Cheat_CalendarDataWindow : MonoBehaviour
{
    public Text displayText;

    void OnEnable()
    {
        if (Calendar.instance == null)
            return;

        ReadOnlyCollection<ScheduledTask> tasks = Calendar.instance.GetScheduledTasks();

        string completeText = "Tasks:\n\n";
        for (int i = 0; i < tasks.Count; i++)
        {
            completeText += tasks[i].ToString() + "\n\n\n";
        }

        displayText.text = completeText;
    }

    void OnDisable()
    {
        displayText.text = "";
    }
}
