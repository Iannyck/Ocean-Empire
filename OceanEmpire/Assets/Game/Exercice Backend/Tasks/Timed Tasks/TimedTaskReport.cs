using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimedTaskReport
{
    public ExerciseTrackingReport exerciseReport;
    public HappyRating rating;

    public DateTime reportCreatedOn;
    public DateTime taskCreatedOn;
    public TimeSlot taskPlannedFor;
    public bool wasInstantTask;

    public TimedTaskReport(){}

    public TimedTaskReport(ExerciseTrackingReport exerciseReport, DateTime taskCreatedOn, TimeSlot plannedTimeSlot, bool isInstantTask, HappyRating rating = HappyRating.None)
    {
        this.exerciseReport = exerciseReport;
        this.rating = rating;
        taskPlannedFor = plannedTimeSlot;
        this.taskCreatedOn = taskCreatedOn;


        reportCreatedOn = DateTime.Now;
    }

    public bool WasCancelled()
    {
        return exerciseReport == null;
    }
}
