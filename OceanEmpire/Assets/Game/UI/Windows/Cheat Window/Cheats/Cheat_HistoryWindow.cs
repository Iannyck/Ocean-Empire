using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

public class Cheat_HistoryWindow : MonoBehaviour
{
    public Text displayText;

    void OnEnable()
    {
        if (Calendar.instance == null)
            return;

        ReadOnlyCollection<TimedTaskReport> tasks = History.instance.GetTaskReports();

        string completeText = "Task Reports:\n\n";
        for (int i = tasks.Count - 1; i >= 0; i--)
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
