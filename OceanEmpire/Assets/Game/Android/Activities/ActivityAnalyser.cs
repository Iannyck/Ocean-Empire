using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityAnalyser : BaseManager<ActivityAnalyser>
{
    public float achieveGap = 50;

    public float timeBetweenUpdate = 2f;

    public float generousMultiplier = 1.3f;

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
        public DateTime firstExerciseDetection;

        public Report() { }

        public Report(TimedTask task)
        {
            complete = false;
            this.task = task;
            activityRate = 0;
            probabilities = null;
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
                if (activities.Count <= 0)
                    Debug.Log("AUCUNE ACTIVITÉ DÉTECTÉ");
                else
                    Debug.Log("ANALYSER GOT SOME ACTIVITIES | " + activities[activities.Count - 1].time);
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
                Report result = new Report(task);
                FillReport(GetAllActiviesInTimeStamp(task.timeSlot.start, until), ref result);
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

    private void FillReport(List<ActivityDetection.Activity> activites, ref Report result)
    {
        if (result.probabilities == null)
            result.probabilities = new List<float>(activites.Count);

        int numberOfCompletion = 0;
        bool firstExerciseDetected = false;
        bool previousWasExercise = false;

        //Default Values
        result.complete = false;


        for (int i = 0; i < activites.Count; i++)
        {
            float prob = activites[i].probability;

            ActivityDetection.Activity nowActivity = activites[i];
            bool nowIsExercise = prob > achieveGap;

            if (nowIsExercise)
            {
                //On compte la quantité d'entré d'exercice
                numberOfCompletion++;

                //A-t-on detecté la première ?
                if (!firstExerciseDetected)
                {
                    result.firstExerciseDetection = nowActivity.time;
                    firstExerciseDetected = true;
                }
            }

            if (previousWasExercise)
            {
                //----------------------Ajout de temps----------------------//

                ActivityDetection.Activity firstActivity = activites[0];
                ActivityDetection.Activity previousActivity = activites[i - 1];

                // Temps à ajouter en considérant le addFactor
                long ticksSinceLastMesure = (nowActivity.time - previousActivity.time).Ticks;
                TimeSpan timeToAdd = new TimeSpan((long)(ticksSinceLastMesure * generousMultiplier));

                // On ajoute le temps de marche au temps total de marche
                result.timeSpendingExercice += timeToAdd;

                TimeSpan timeSinceStart = nowActivity.time - firstActivity.time;

                // Si le nouveau temps est plus grand que le temps total entre le début de l'exercise jusqu'au dernier
                if (result.timeSpendingExercice > timeSinceStart)
                {
                    // Le temps total doit être égal au temps temps total entre le début de l'exercise jusqu'au dernier
                    result.timeSpendingExercice = timeSinceStart;
                }// Sinon le temps total est correct on a pas dépassé le temps max de l'exercise


                //----------------------A-t-on terminé ?----------------------//
                TimeSpan objective = ((WalkTask)result.task.task).timeOfWalk;

                if (result.timeSpendingExercice >= objective)
                {
                    result.complete = true;
                    break;
                }
            }

            previousWasExercise = nowIsExercise;
            result.exerciceEnd = nowActivity.time;

            result.probabilities.Add(prob);
        }


        result.activityRate = numberOfCompletion / (float)activities.Count;
        result.produceTime = DateTime.Now;
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
        Debug.Log("ASKING FOR THE ACTIVITIES");
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

    public ActivityDetection.Activity GetLast()
    {
        if (activities.Count > 1)
            return activities.Last();
        return null;
    }
}
