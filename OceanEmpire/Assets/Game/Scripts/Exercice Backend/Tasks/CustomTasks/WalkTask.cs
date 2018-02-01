using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WalkTask : Task
{
    public float minutesOfWalk { get { return (float)timeOfWalk.TotalSeconds / 60f; } }
    public TimeSpan timeOfWalk;

    public override ExerciseType GetExerciseType()
    {
        return ExerciseType.Walk;
    }

    public WalkTask(float minutesOfWalk)
    {
        timeOfWalk = new TimeSpan(0, 0, minutesOfWalk.RoundedToInt() * 60);
    }

    public static WalkTask Build(float difficulty)
    {
        return new WalkTask(difficulty);
    }

    public override TimeSpan GetAllocatedTime()
    {
        double exerciseTime = timeOfWalk.TotalSeconds * 1.5;
        return new TimeSpan(0, 15, 0) + TimeSpan.FromSeconds(exerciseTime);
        //return new TimeSpan(0, 0, 15); // TEMPORAIRE
    }

    public override string ToString()
    {
        return minutesOfWalk + "min walk:\n" + base.ToString();
    }
}
