using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExerciseTrackingReport
{
    public enum State { Stopped = 0, Completed = 1, UserSaidItWasCompleted = 2 }

    public float completionRate;
    public State state;
    public CalendarTime startTime;
    public CalendarTime endTime;

    public ExerciseTrackingReport(State state, float completionRate, CalendarTime startTime, CalendarTime endTime)
    {
        this.state = state;
        this.completionRate = completionRate;
        this.startTime = startTime;
        this.endTime = endTime;
    }
}
