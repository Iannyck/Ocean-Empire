using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantTask : ScheduledTask
{
    public override TaskReport BuildReport()
    {
        return new TaskReport(null, DateTime.Now, plannedOn.dateTime);
    }

    public InstantTask(Task task)
        :base(task, new CalendarTime(DateTime.Now))
    {
    }
}
