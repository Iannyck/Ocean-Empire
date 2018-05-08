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
        textComponent.text = timeSlot.ToCondensedDayOfTimeString() + "   " + label;
    }
    public void FillContent(string label, TimeSlot timeSlot)
    {
        textComponent.text = label + "   " + timeSlot.ToCondensedDayOfTimeString();
    }

    public void FillContent(Schedule schedule)
    {
        TimeSlot slot = schedule.timeSlot;
        string label = " - ";
        if (schedule.task != null)
        {
            label = "<color=#f>" + schedule.task.minDuration + " à " + schedule.task.maxDuration + " min de marche</color>";
        }

        FillContent(slot, label);
    }
}
