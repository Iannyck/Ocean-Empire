using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantTask : ScheduledTask
{

    public override TimedTaskReport BuildReport(ExerciseTrackingReport trackingReport, HappyRating happyRating)
    {
        return new TimedTaskReport(trackingReport, createdOn, timeSlot, true, happyRating);
    }

    public InstantTask(Task task)
        :base(task, DateTime.Now)
    {
    }
}
