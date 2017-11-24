using System;
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
    public List<float> probabilities;
    public TimeSpan timeSpendingExercice;

    public ExerciseTrackingReport(State state, List<float> probabilities, TimeSpan timeSpendingExercice, float completionRate, CalendarTime startTime, CalendarTime endTime)
    {
        this.state = state;
        this.completionRate = completionRate;
        this.startTime = startTime;
        this.endTime = endTime;
        this.probabilities = probabilities;
        this.timeSpendingExercice = timeSpendingExercice;
    }
}
