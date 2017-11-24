using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InstantTask : ScheduledTask
{
    public override TimedTaskReport BuildReport()
    {
        return new TimedTaskReport(null, createdOn, timeSlot, true);
    }

    public InstantTask(Task task) : base(task, DateTime.Now)
    {
    }

    public override bool IsVisibleInCalendar()
    {
        return false;
    }
}
