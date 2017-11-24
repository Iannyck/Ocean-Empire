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
    public CalendarTime taskPlannedFor;
    public bool wasInstantTask;

    public TimedTaskReport(){}

    public TimedTaskReport(ExerciseTrackingReport exerciseReport, DateTime taskCreatedOn, CalendarTime plannedFor, bool isInstantTask, HappyRating rating = HappyRating.None)
    {
        this.exerciseReport = exerciseReport;
        this.rating = rating;
        reportCreatedOn = DateTime.Now;
        taskPlannedFor = plannedFor;
        this.taskCreatedOn = taskCreatedOn;
    }

    public bool WasCancelled()
    {
        return exerciseReport == null;
    }
}
