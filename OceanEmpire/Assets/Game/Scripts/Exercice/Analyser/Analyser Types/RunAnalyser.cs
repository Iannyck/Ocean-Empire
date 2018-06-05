using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAnalyser : StandardGoogleAnalyser
{
    public override PrioritySheet.ExerciseTypes GoogleExerciseType
    {
        get
        {
            return PrioritySheet.ExerciseTypes.run;
        }
    }

    public override ExerciseType ExerciseType
    {
        get
        {
            return ExerciseType.Run;
        }
    }
}