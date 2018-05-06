﻿using System;
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
            label = "<color=#f>" + schedule.task.requiredExerciseVolume + " à " + schedule.task.maxDuration + " min de marche</color>";
        }

        FillContent(slot, label);
    }
}
