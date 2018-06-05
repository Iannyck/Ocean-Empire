using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeAnalyser : StandardGoogleAnalyser
{
    public override PrioritySheet.ExerciseTypes GoogleExerciseType
    {
        get
        {
            return PrioritySheet.ExerciseTypes.bicycle;
        }
    }

    public override ExerciseType ExerciseType
    {
        get
        {
            return ExerciseType.Bike;
        }
    }
}

