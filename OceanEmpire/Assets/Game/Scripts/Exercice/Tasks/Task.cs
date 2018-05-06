using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Task
{
    public float requiredExerciseVolume;
    public float maxDuration;
    public int ticketReward;

    public Task(float requiredExerciseVolume, float taskMaxDuration, int ticketReward)
    {
        this.requiredExerciseVolume = requiredExerciseVolume;
        this.maxDuration = taskMaxDuration;
        this.ticketReward = ticketReward;
    }

    public TimeSpan GetMaxDurationAsTimeSpan()
    {
        return new TimeSpan(0, 0, Mathf.RoundToInt(maxDuration * 60));
    }

    public override string ToString()
    {
        return "Task: volumeRequired(" + requiredExerciseVolume + ")  maxDuration(" + maxDuration + ")  ticketReward(" + ticketReward + ")";
    }
}