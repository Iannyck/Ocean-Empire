using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTracker : ExerciseTracker
{
    public override ExerciseType GetExerciseType()
    {
        return ExerciseType.Walk;
    }
}
