using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskReport
{
    public ExerciseTrackingReport exerciseReport;
    public HappyRating rating;

    public CalendarTime createdAt;
    public CalendarTime plannedFor;

    public TaskReport(){}

    public TaskReport(ExerciseTrackingReport exerciseReport, DateTime createdAt, DateTime plannedFor, HappyRating rating = HappyRating.None)
    {
        this.exerciseReport = exerciseReport;
        this.rating = rating;
        this.createdAt = new CalendarTime(createdAt);
        this.plannedFor = new CalendarTime(plannedFor);
    }

    public bool WasInstantTask()
    {
        return plannedFor == createdAt;
    }
    public bool WasCancelled()
    {
        return exerciseReport == null;
    }
}
