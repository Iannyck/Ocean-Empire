using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExerciseTracker
{
    // Deux exemple de fonction qui pourrait se retrouver ici.

    public CalendarTime GetTimeSinceStart()
    {
        throw new System.NotImplementedException();
    }
    public CalendarTime GetStartTime()
    {
        throw new System.NotImplementedException();
    }

    public abstract ExerciseType GetExerciseType();
}
