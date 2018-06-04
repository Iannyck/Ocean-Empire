using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayInspector_Schedule : MonoBehaviour
{
    [SerializeField] private Text headerText;
    [SerializeField] private Text descriptionText;

    public void FillContent(TimeSlot timeSlot, string label, string description)
    {
        headerText.text = timeSlot.ToCondensedDayOfTimeString() + "   " + label;
        descriptionText.text = description;
    }

    public void FillContent(Schedule schedule)
    {
        TimeSlot slot = schedule.timeSlot;
        string label = " - ";
        if (schedule.task != null)
        {
            label = "Faites " + schedule.task.minDuration + " à " + schedule.task.maxDuration + " min de marche"
                + " pour obtenir " + schedule.task.ticketReward + " tickets!";
        }

        FillContent(slot, "", label);
    }
}
