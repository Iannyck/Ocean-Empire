using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityAnalyser : BaseManager<ActivityAnalyser>
{
    public float achieveGap = 50;

    public float timeBetweenUpdate = 2f;

    [HideInInspector]
    public bool waitingForDataUpdate = false;

    [HideInInspector]
    public List<ActivityDetection.Activity> activities = new List<ActivityDetection.Activity>();

    public class Report
    {
        public float activityRate; // dans toute le temps que ta passé a faire de l'exercice, à quel point t'en a vraiment faites
        public List<float> probabilities;
        public DateTime produceTime;
        public DateTime exerciceEnd;
        public TimedTask task;
        public TimeSpan timeSpendingExercice;
        public bool complete;

        public Report() { }

        public Report(TimedTask task)
        {
            complete = false;
            this.task = task;
            activityRate = 0;
            probabilities = new List<float>();
            exerciceEnd = DateTime.Now;
            produceTime = DateTime.Now;
            timeSpendingExercice = new TimeSpan(0, 0, 0);
        }
    }

    public override void Init()
    {
        CompleteInit();
        UpdateActivities();
    }

    private void UpdateActivities()
    {
        if (waitingForDataUpdate)
            return;

        waitingForDataUpdate = true;
        AskForActivities(delegate (List<ActivityDetection.Activity> activities)
        {
            if (activities != null)
            {
                Debug.Log("ANALYSER GOT SOME ACTIVITIES");
                waitingForDataUpdate = false;
                this.activities = activities;
                DelayManager.LocalCallTo(UpdateActivities, Mathf.Max(timeBetweenUpdate, 0.5f), this);
            }
            else
            {
                Debug.Log("ANALYSER FAILED TO GET SOME ACTIVITIES");
                waitingForDataUpdate = true;
                UpdateActivities();
            }
        });
    }

    public Report VerifyCompletion(TimedTask task)
    {
        return VerifyCompletion(task, DateTime.Now);
    }

    public Report VerifyCompletion(TimedTask task, DateTime until)
    {
        switch (task.task.GetExerciseType())
        {
            case ExerciseType.Walk:
                Debug.Log("BEGIN VERIFICATION FOR WALK");
                Report result = new Report(task);
                GetReport(GetAllActiviesInTimeStamp(task.timeSlot.start,until), ref result);
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
    }

    private void GetReport(List<ActivityDetection.Activity> activites, ref Report result)
    {
        result.probabilities = new List<float>();

        float numberOfActivities = activites.Count;
        float numberOfCompletion = 0;

        bool doingExercice = false;
        DateTime lastExercice = DateTime.Now;

        for (int i = 0; i < numberOfActivities; i++)
        {
            float prob = activites[i].probability;
            // On est en train de faire un exercice
            if (prob > achieveGap)
            {
                numberOfCompletion++;
                // On vient de commencer
                if (!doingExercice)
                {
                    doingExercice = true;
                    lastExercice = activites[i].time;
                } else
                { // On continue
                    result.timeSpendingExercice = result.timeSpendingExercice.Add(activites[i].time.Subtract(lastExercice));
                    lastExercice = activites[i].time;
                }
            }
            else
            {
                // On vient d'arrêter de faire un exercice
                if (doingExercice)
                {
                    result.timeSpendingExercice = result.timeSpendingExercice.Add(activites[i].time.Subtract(lastExercice));
                    doingExercice = false;
                    if (result.timeSpendingExercice.CompareTo(new TimeSpan(0, (int)((WalkTask)result.task.task).minutesOfWalk, 0)) == 1)
                    {
                        result.exerciceEnd = activites[i].time;
                        result.complete = true;

                        if (numberOfActivities == 0)
                            result.activityRate = 0;
                        else
                            result.activityRate = (numberOfCompletion / numberOfActivities);

                        result.produceTime = DateTime.Now;

                        return;
                    }
                }
            }
            result.probabilities.Add(prob);
        }
    }

    public List<ActivityDetection.Activity> GetAllActiviesInTimeStamp(DateTime start, DateTime end, ExerciseType type = ExerciseType.Walk)
    {
        if (start.CompareTo(end) >= 1)
            return null;
        if (end.CompareTo(DateTime.Now) >= 1)
            return null;

        List<ActivityDetection.Activity> result = activities;
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
                    if (type != ExerciseType.Walk)
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

    public void AskForActivities(Action<List<ActivityDetection.Activity>> onComplete = null, ExerciseType type = ExerciseType.Walk)
    {
        List<ActivityDetection.Activity> result = null;
        ActivityDetection.LoadActivities(delegate (List<ActivityDetection.Activity> outputActivities)
        {
            result = outputActivities;
            if (result == null)
            {
                onComplete.Invoke(null);
                return;
            }

            //Clear la vidange (on garde slmt la marche)
            for (int i = 0; i < result.Count; i++)
            {
                switch (result[i].type)
                {
                    case ActivityDetection.Activity.ActivityType.Walking:
                        if (type != ExerciseType.Walk)
                        {
                            result.Remove(result[i]);
                            i--;
                            if (result.Count <= 0)
                                onComplete.Invoke(result);
                        }
                        break;
                    default:
                        break;
                }
            }
            onComplete.Invoke(result);
        });
    }

    public void ResetActivities()
    {
        activities = new List<ActivityDetection.Activity>();
    }
}
