using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTracker : ExerciseTracker
{
    public override ExerciseType GetExerciseType()
    {
        return ExerciseType.Walk;
    }

    public override ActivityAnalyser.Report UpdateTracking(TimedTask task, DateTime startedWhen)
    {
        return null;
    }

    public override ActivityAnalyser.Report ForceCompletion(TimedTask task)
    {
        return null;
    }
}
