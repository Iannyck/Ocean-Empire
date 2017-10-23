using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WalkTask : Task
{
    public override ExerciseType GetExerciseType()
    {
        return ExerciseType.Walk;
    }
}
