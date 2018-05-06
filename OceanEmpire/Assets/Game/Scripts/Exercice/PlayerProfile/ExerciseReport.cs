using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseReport
{
    public TimeSlot timeSlot;
    public float volume;
    public Task task;
    public State state;

    public enum State
    {
        Completed = 0,
        Failed = 1
    }

    public ExerciseReport(TimeSlot timeSlot, Task exercice, float volume)
    {
        this.timeSlot = timeSlot;
        this.volume = volume;
        this.task = exercice;
        state = State.Failed;
    }

    public ExerciseReport()
    {
        timeSlot = new TimeSlot();
        volume = 0;
        state = State.Failed;
        task = null;
    }

    public string GetString()
    {
        return "|" + timeSlot.ToString() + "-"+ task.requiredExerciseVolume + "-" + task.maxDuration + "-" + volume + "-" + state;
    }
}
