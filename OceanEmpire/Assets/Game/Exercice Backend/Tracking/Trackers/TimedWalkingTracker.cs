using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedWalkingTracker : WalkTracker {

    public override ActivityAnalyser.Report UpdateTracking(TimedTask task, DateTime startedWhen)
    {
        if (task == null)
        {
            Debug.Log("TimedTask NULL");
            return null;
        }
        if (((WalkTask)task.task) == null)
        {
            Debug.Log("Not a WalkTask");
            return null;
        }

        return ActivityAnalyser.VerifyCompletion(task);

        /*
        // Temps courrant - Temps au debut est plus grand que le Temps au debut + minutesOfWalk
        TimeSpan toCompare = (startedWhen.AddMinutes(((WalkTask)task.task).minutesOfWalk)).Subtract(startedWhen);
        if ((DateTime.Now.Subtract(startedWhen)).CompareTo(toCompare) >= 1)
            return ActivityAnalyser.VerifyCompletion(task);
        return null;
        */
    }

    public override ActivityAnalyser.Report EvaluateTask(TimedTask task)
    {
        return ActivityAnalyser.VerifyCompletion(task);
    }

    // Exemple de tracking
    //
    // START
    // ExerciseTracker tracker = ExerciseComponents.GetTracker(ExerciseType.Walk)
    // 
    // UPDATE
    // ActivityAnalyser.Report currentReport = tracker.UpdateTracking(task, startedWhen) // task=TimedTask, startedWhen=DateTime
    // if(currentReport != null)
    //     REPORT = ActivityAnalyser.ProduceReport(currentReport,state) // exercise complete ! state=ExerciseTrackingReport.State 
    // finito
}
