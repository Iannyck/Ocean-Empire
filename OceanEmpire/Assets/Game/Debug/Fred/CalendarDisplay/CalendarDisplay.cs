using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CalendarUI;
using System;

public class CalendarDisplay : MonoBehaviour
{
    public Day[] daysDisplay;
    
    private Calendar currentCalendar;
    private int monthOffset = 0;
    private DateTime today = DateTime.Now;
    private const int TOTAL_DISPLAYED_WEEKS = 5;

    public void DisplayCalendar(Calendar calendar)
    {
        currentCalendar = calendar;
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        List<Calendar.Day> calendarDays = Calendar.GetWeeksOfMonth(today, TOTAL_DISPLAYED_WEEKS, monthOffset);

        int Count = Mathf.Min(daysDisplay.Length, calendarDays.Count);
        for (int i = 0; i < Count; i++)
        {
            daysDisplay[i].DisplayDay(calendarDays[i]);
        }
    }
}
