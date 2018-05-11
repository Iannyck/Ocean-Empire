using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using UnityEngine.UI;
using System.Text;

public class Cheat_CalendarDataWindow : MonoBehaviour
{
    public Text displayText;

    void OnEnable()
    {
        if (Calendar.instance == null)
            return;

        var past = Calendar.instance.GetPastSchedules();
        var presentAndFuture = Calendar.instance.GetPresentAndFutureSchedules();

        StringBuilder completeText = new StringBuilder("PAST SCHEDULES:\n\n");
        for (int i = past.Count - 1; i >= 0; i--)
        {
            completeText.Append(past[i].ToString()).Append("\n\n\n");
        }

        completeText.Append("\n\nPRESENT AND FUTURE SCHEDULES:\n\n");
        for (int i = presentAndFuture.Count - 1; i >= 0; i--)
        {
            completeText.Append(presentAndFuture[i].ToString()).Append("\n\n\n");
        }

        displayText.text = completeText.ToString();
    }

    void OnDisable()
    {
        displayText.text = "";
    }
}
