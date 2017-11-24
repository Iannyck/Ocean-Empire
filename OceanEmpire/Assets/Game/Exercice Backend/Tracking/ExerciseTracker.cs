using System;
<<<<<<< .merge_file_a05860
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

    public abstract ActivityAnalyser.Report UpdateTracking(TimedTask task, DateTime startedWhen);

    public abstract ExerciseType GetExerciseType();

    public abstract ActivityAnalyser.Report ForceCompletion(TimedTask task);
}
=======
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

    public abstract ActivityAnalyser.Report UpdateTracking(TimedTask task, DateTime startedWhen);

    public abstract ExerciseType GetExerciseType();

    public abstract ActivityAnalyser.Report ForceCompletion(TimedTask task);
}
>>>>>>> .merge_file_a10040
