using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WalkTask : Task
{
    public float minutesOfWalk;

    public override ExerciseType GetExerciseType()
    {
        return ExerciseType.Walk;
    }

    public WalkTask(float minutesOfWalk)
    {
        this.minutesOfWalk = minutesOfWalk;
    }

    public static WalkTask Build(float difficulty)
    {
        return new WalkTask(2);
    }
}
