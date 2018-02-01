using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExerciseTracker
{
    // Deux exemple de fonction qui pourrait se retrouver ici.

    public TimeSlot GetTimeSinceStart()
    {
        throw new System.NotImplementedException();
    }
    public TimeSlot GetStartTime()
    {
        throw new System.NotImplementedException();
    }

    public abstract ActivityAnalyser.Report Track(TimedTask task, bool untilNow = true);

    public abstract ExerciseType GetExerciseType();
}
