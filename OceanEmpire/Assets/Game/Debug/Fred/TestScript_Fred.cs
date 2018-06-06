using UnityEngine;
using Questing;
using System;
using System.Collections.Generic;

public class TestScript_Fred : MonoBehaviour
{
    public WalkAnalyser walkAnalyser;
    public Transform leftBound;
    public Transform rightBound;

    public Transform pointHolder;

    void Start()
    {
        PersistentLoader.LoadIfNotLoaded();
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
    }

    void Update()
    {
        //List<GoogleActivities.ActivityReport> list = new List<GoogleActivities.ActivityReport>();
        //foreach (Transform child in pointHolder)
        //{
        //    list.Add(new GoogleActivities.ActivityReport()
        //    {
        //        best = new GoogleActivities.ActivityReport.BestActivity()
        //        {
        //            rate = 100,
        //            type = child.position.y > 0 ? PrioritySheet.ExerciseTypes.walk : PrioritySheet.ExerciseTypes.run
        //        },
        //        time = TransformToDateTime(child),
        //        backupActivity = null
        //    });
        //}
        //var report = walkAnalyser.GetExerciseVolume(new TimeSlot(TransformToDateTime(leftBound), TransformToDateTime(rightBound)), list);
        //if (report != null)
        //    Debug.Log(report.volume);
    }

    DateTime TransformToDateTime(Transform tr)
    {
        return new DateTime(2000, 1, 1, 0, 0, 0) + new TimeSpan(0, 0, Mathf.RoundToInt(tr.position.x * 60));
    }

}