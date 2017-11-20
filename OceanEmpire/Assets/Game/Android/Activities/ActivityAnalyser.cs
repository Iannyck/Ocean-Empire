using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityAnalyser {

    const float achieveGap = 60;

    public class Report
    {
        public float completionRate;
        public List<float> probabilities;
        public DateTime produceTime;
        public TimedTask task;

        public Report() { }

        public Report(TimedTask task) { this.task = task; }
    }

    public static Report VerifyCompletion(TimedTask task)
    {
        return VerifyCompletion(task, DateTime.Now);
    }

    public static Report VerifyCompletion(TimedTask task, DateTime until)
    {
        switch (task.task.GetExerciseType())
        {
            case ExerciseType.Walk:
                List<ActivityDetection.Activity> activites = GetAllActiviesInTimeStamp(task.plannedOn.dateTime, until);
                if (activites == null)
                    break;
                Report result = new Report(task);
                GetReport(activites,ref result);
                return result;
            case ExerciseType.Run:
                // TODO
                return new Report();
            case ExerciseType.Stairs:
                // TODO
                return new Report();
            default:
                return new Report();
        }
        return new Report();
    }

    private static void GetReport(List<ActivityDetection.Activity> activites,ref Report result)
    {
        result.probabilities = new List<float>();

        float numberOfActivities = activites.Count;
        float numberOfCompletion = 0;

        for (int i = 0; i < numberOfActivities; i++)
        {
            float prob = activites[i].probability;
            if (prob >= achieveGap)
                numberOfCompletion++;
            result.probabilities.Add(prob);
        }
        result.completionRate = (numberOfCompletion / numberOfActivities);
        result.produceTime = DateTime.Now;
    }

    public static List<ActivityDetection.Activity> GetAllActiviesInTimeStamp(DateTime start, DateTime end, ExerciseType type = ExerciseType.Walk)
    {
        if(start.CompareTo(end) >= 1)
            return null;
        if (end.CompareTo(DateTime.Now) >= 1)
            return null;

        List<ActivityDetection.Activity> result = ActivityDetection.LoadActivities();
        for (int i = 0; i < result.Count; i++)
        {
            if (result[i].time.CompareTo(start) <= -1 || result[i].time.CompareTo(end) >= 1)
                result.Remove(result[i]);
            switch (result[i].type)
            {
                case ActivityDetection.Activity.ActivityType.Walking:
                    if(type != ExerciseType.Walk)
                        result.Remove(result[i]);
                    break;
                default:
                    break;
            }
        }

        return result;
    }

    public static ExerciseTrackingReport ProduceReport(Report report, ExerciseTrackingReport.State state)
    {
        return new ExerciseTrackingReport(state, report.completionRate, report.task.plannedOn, new CalendarTime(report.produceTime));
    }
}
