using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript_Fred : MonoBehaviour
{
    public List<int> list = new List<int>();
    public Calendar calendar = new Calendar();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ScheduledTask st = new ScheduledTask();
            st.plannedOn = new CalendarTime(DateTime.Now.AddDays(2));
            calendar.AddScheduledTask(st);
            print(calendar.RemoveScheduledTask(st));
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            List<Calendar.Week> weeks = Calendar.GetFiveWeeksOfTheMonth(DateTime.Now.AddMonths(-1));
            print(weeks.Count);
        }
    }
}
