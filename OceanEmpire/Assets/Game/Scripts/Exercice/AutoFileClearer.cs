using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFileClearer : MonoBehaviour
{
    
    void Start()
    {
        this.DelayedCall(() =>
        {
            PlannedExerciceRewarder.Report report = PlannedExerciceRewarder.Instance.LatestPendingReport;
            if (report == null)
            {
                Clear();
            }
        }, 0.75f);
    }

    void Clear()
    {
        ContinuousRewarder.Instance.ForceUpdate();
        GoogleActivities.instance.ClearAllActivitiesSave(false);
    }
}
