using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarGrid_Week : MonoBehaviour
{
    public CalendarGrid_Day[] days;
    public void Fill(List<Calendar.Day> days)
    {
        DateTime now = DateTime.Now;
        Calendar.Day today = new Calendar.Day(now);
        for (int i = 0; i < days.Count; i++)
        {
            this.days[i].Fill(days[i], days[i] == today);
        }
    }
    public Calendar.Day GetCalendarDay() { return days[0].GetCalendarDay(); }
}
