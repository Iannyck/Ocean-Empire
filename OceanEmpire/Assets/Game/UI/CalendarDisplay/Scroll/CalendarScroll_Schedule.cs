using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarScroll_Schedule : MonoBehaviour
{
    [SerializeField] private Text textComponent;

    public void FillContent(TimeSlot timeSlot, string label)
    {
        textComponent.text = TimeslotToString(timeSlot) + "   " + label;
    }

    static string TimeslotToString(TimeSlot timeSlot)
    {
        return DateTimeToString(timeSlot.start) + " - " + DateTimeToString(timeSlot.end);
    }
    static string DateTimeToString(DateTime dateTime)
    {
        string minutes = dateTime.Minute.ToString();
        if (minutes.Length < 2)
            minutes.Insert(0, "0");
        return dateTime.Hour + "h" + minutes;
    }
}
