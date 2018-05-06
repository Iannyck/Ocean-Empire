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

    public void FillContent(ScheduledBonus schedule)
    {
        TimeSlot slot = schedule.timeSlot;
        if (schedule.displayPadding)
        {
            TimeSpan padding = new TimeSpan(0, 0, Mathf.RoundToInt(schedule.minutesOfPadding * 60));
            slot.start -= padding;
            slot.end += padding;
        }
        string label = " - ";
        if (schedule.task != null)
        {
            label = "Faites " + schedule.task.minDuration + " à " + schedule.task.maxDuration + " min de marche"
                + " pour obtenir " + schedule.task.ticketReward + " tickets!";
        }

        FillContent(slot, "", label);
    }
}
