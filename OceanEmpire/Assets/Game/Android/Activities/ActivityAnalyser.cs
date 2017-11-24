using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityAnalyser {

    public const float achieveGap = 50;

    public class Report
    {
        public float activityRate; // dans toute le temps que ta passé a faire de l'exercice, à quel point t'en a vraiment faites
        public List<float> probabilities;
        public DateTime produceTime;
        public TimedTask task;
        public TimeSpan timeSpendingExercice;
        public bool complete;

        public Report() { }

        public Report(TimedTask task) {
            complete = false;
            this.task = task;
            activityRate = 0;
            probabilities = new List<float>();
            produceTime = DateTime.Now;
        }
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
                return new Report(task);
            case ExerciseType.Stairs:
                // TODO
                return new Report(task);
            default:
                return new Report(task);
        }
        return new Report(task);
    }

    private static void GetReport(List<ActivityDetection.Activity> activites,ref Report result)
    {
        result.probabilities = new List<float>();

        float numberOfActivities = activites.Count;
        float numberOfCompletion = 0;

        bool doingExercice = false;
        DateTime lastExercice = DateTime.Now;

        for (int i = 0; i < numberOfActivities; i++)
        {
            float prob = activites[i].probability;
            if (prob >= achieveGap)
            {
                if (!doingExercice)
                    doingExercice = true;
                else
                    result.timeSpendingExercice.Add(activites[i].time.Subtract(lastExercice));
                lastExercice = activites[i].time;
                numberOfCompletion++;
            }
            else
            {
                result.timeSpendingExercice.Add(activites[i].time.Subtract(lastExercice));
                doingExercice = false;
            }
            result.probabilities.Add(prob);
        }

        if (numberOfActivities == 0)
            result.activityRate = 0;
        else
            result.activityRate = (numberOfCompletion / numberOfActivities);

        if (result.timeSpendingExercice.CompareTo(new TimeSpan(0,(int)((WalkTask)result.task.task).minutesOfWalk,0)) == 1)
            result.complete = true;

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
            {
                result.Remove(result[i]);
                i--;
                if (result.Count <= 0)
                    return result;
                continue;
            }
            switch (result[i].type)
            {
                case ActivityDetection.Activity.ActivityType.Walking:
                    if(type != ExerciseType.Walk)
                    {
                        result.Remove(result[i]);
                        i--;
                        if (result.Count <= 0)
                            return result;
                    }
                    break;
                default:
                    break;
            }
        }

        return result;
    }

    public static ExerciseTrackingReport ProduceReport(Report report, ExerciseTrackingReport.State state)
    {
        return new ExerciseTrackingReport(state, report.probabilities, report.timeSpendingExercice, report.activityRate, report.task.plannedOn, new CalendarTime(report.produceTime));
    }
}
