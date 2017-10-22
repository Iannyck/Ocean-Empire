using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTask_Duration : Task
{
    public float duration;

    public override ExerciseType GetExerciseType()
    {
        return ExerciseType.Walk;
    }
}
