using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskReport
{
    public ExerciseTrackingReport exerciseReport;
    public HappyRating rating = HappyRating.None;

    public CalendarTime createdAt;
    public CalendarTime plannedFor;

    public bool WasInstantTask()
    {
        return plannedFor == createdAt;
    }
    public bool WasCancelled()
    {
        return exerciseReport == null;
    }
}
