using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduledTask : TimedTask
{
    public override TaskReport BuildReport()
    {
        throw new NotImplementedException();
    }

    public ScheduledTask(Task task, CalendarTime plannedOn)
    {
        this.task = task;
        this.plannedOn = plannedOn;
    }
}
