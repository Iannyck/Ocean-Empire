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

        return ActivityAnalyser.instance.VerifyCompletion(task);
    }

    public override ActivityAnalyser.Report EvaluateTask(TimedTask task)
    {
        return ActivityAnalyser.instance.VerifyCompletion(task);
    }
}
