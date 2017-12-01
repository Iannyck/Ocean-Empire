using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedWalkingTracker : WalkTracker {

    public override ActivityAnalyser.Report Track(TimedTask task, bool untilNow = true)
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

        // TODO FAIRE LA LOGIQUE TU TEMPS ICI

        if(untilNow)
            return ActivityAnalyser.instance.VerifyCompletion(task);
        else
            return ActivityAnalyser.instance.VerifyCompletion(task, task.timeSlot.end);
    }
}
