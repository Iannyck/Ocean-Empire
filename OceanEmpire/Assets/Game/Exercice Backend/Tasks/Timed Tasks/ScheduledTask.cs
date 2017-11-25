using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScheduledTask : TimedTask
{
    public virtual TimeSpan GetTotalAllocatedTime()
    {
        return new TimeSpan(0, 15, 0);
    }

    public override TimedTaskReport BuildReport(ExerciseTrackingReport trackingReport, HappyRating happyRating)
    {
        throw new NotImplementedException();
    }

    public ScheduledTask(Task task, DateTime plannedOn)
    {
        this.task = task;
        createdOn = DateTime.Now;
        timeSlot = new TimeSlot(plannedOn, task.GetAllocatedTime());
    }
    
    public override bool IsVisibleInCalendar()
    {
        return true;
    }
}
