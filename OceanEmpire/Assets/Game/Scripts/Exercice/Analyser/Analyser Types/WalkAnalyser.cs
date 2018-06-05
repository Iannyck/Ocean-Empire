using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAnalyser : StandardGoogleAnalyser
{
    public override PrioritySheet.ExerciseTypes GoogleExerciseType
    {
        get
        {
            return PrioritySheet.ExerciseTypes.walk;
        }
    }

    public override ExerciseType ExerciseType
    {
        get
        {
            return ExerciseType.Walk;
        }
    }
}
