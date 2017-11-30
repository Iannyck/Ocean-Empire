using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WalkTask : Task
{
    public float minutesOfWalk;
    public TimeSpan timeOfWalk;

    public override ExerciseType GetExerciseType()
    {
        return ExerciseType.Walk;
    }

    public WalkTask(float minutesOfWalk)
    {
        this.minutesOfWalk = minutesOfWalk;
        timeOfWalk = new TimeSpan(0, (int)minutesOfWalk, 0);
    }

    public static WalkTask Build(float difficulty)
    {
        return new WalkTask(difficulty);
    }

    public override TimeSpan GetAllocatedTime()
    {
        double exerciseTime = timeOfWalk.TotalSeconds * 1.5;
        return new TimeSpan(0, 15, 0) + TimeSpan.FromSeconds(exerciseTime);
    }
}
