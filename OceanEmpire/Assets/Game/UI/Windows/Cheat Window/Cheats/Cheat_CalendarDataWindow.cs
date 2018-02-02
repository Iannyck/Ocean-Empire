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

        //ReadOnlyCollection<ScheduledTask> tasks = Calendar.instance.GetScheduledTasks();

        //string completeText = "Tasks:\n\n";
        //for (int i = tasks.Count - 1; i >= 0; i--)
        //{
        //    completeText += tasks[i].ToString() + "\n\n\n";
        //}

        displayText.text = "FIX DAT SHIT BOI";
    }

    void OnDisable()
    {
        displayText.text = "";
    }
}
