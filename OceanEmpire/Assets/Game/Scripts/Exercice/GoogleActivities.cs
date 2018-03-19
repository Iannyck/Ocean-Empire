 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Persistence;

public class GoogleActivities : MonoPersistent
{
    // Un enregistrement dans le temps
    public class ActivityReport
    {
        public BestActivity best;
        public DateTime time;
        public GoogleReader.Activity backupActivity;

        public ActivityReport() { }

        public struct BestActivity
        {
            public int rate;
            public PrioritySheet.ExerciseTypes type;
        }
    }

    // Parametres
    public PrioritySheet priority;
    public float timeBetweenUpdate = 2f;
    private float minTimeBetweenUpdate = 0.5f;
    private bool waitingForDataUpdate = false;

    // Data
    public List<GoogleReader.Activity> activities = new List<GoogleReader.Activity>();
    public List<ActivityReport> records = new List<ActivityReport>();

    // Instance
    static public GoogleActivities instance;

    public override void Init(Action onComplete)
    {
        instance = this;
        UpdateRecord();
        onComplete();
    }

    private void UpdateRecord()
    {
        if (waitingForDataUpdate)
            return;

        waitingForDataUpdate = true;
        AskForActivities(delegate (List<GoogleReader.Activity> activities)
        {
            if (activities != null)
            {
                waitingForDataUpdate = false;
                this.activities = activities;
                CreateRecord();
            }
            else
            {
                waitingForDataUpdate = true;
            }
            this.DelayedCall(UpdateRecord, Mathf.Max(timeBetweenUpdate, minTimeBetweenUpdate));
        });
    }

    private void AskForActivities(Action<List<GoogleReader.Activity>> onComplete = null)
    {
        List<GoogleReader.Activity> result = null;

        GoogleReader.LoadActivities(delegate (List<GoogleReader.Activity> outputActivities)
        {
            result = outputActivities;
            if (result == null)
            {
                onComplete.Invoke(null);
                return;
            }
            onComplete.Invoke(result);
        });
    }

    private void CreateRecord()
    {
        records = new List<ActivityReport>();
        for (int i = 0; i < activities.Count; i++)
        {
            GoogleReader.Activity currentActivity = activities[i];

            ActivityReport currentReport = new ActivityReport();
            currentReport.backupActivity = currentActivity;

            ActivityReport.BestActivity currentBest = new ActivityReport.BestActivity();
            currentBest.rate = -1;

            for (int j = 0; j < activities[i].probabilities.Count; j++)
            {
                int currentProb = activities[i].probabilities[j];
                PrioritySheet.ExerciseTypes currentType = currentActivity.GetActivityByIndex(j);

                if (currentBest.rate == -1)
                {
                    currentBest.rate = currentProb;
                    currentBest.type = currentType;
                }
                else if (currentBest.rate < currentProb){
                    currentBest.rate = currentProb;
                    currentBest.type = currentType;
                }
                else if(currentBest.rate == currentProb) {
                    int result = priority.Compare(currentBest.type, currentType);
                    if(result == 1)
                    {
                        currentBest.rate = currentProb;
                        currentBest.type = currentType;
                    }
                }
            }
            currentReport.best = currentBest;
            currentReport.time = currentActivity.time;

            records.Add(currentReport);
        }
    }

    public void ResetActivities()
    {
        activities = new List<GoogleReader.Activity>();
        records = new List<ActivityReport>();
    }

    public void ClearAllActivitiesSave()
    {
        activities.Clear();
        records.Clear();
        GoogleReader.ResetActivitiesSave();
    }

    // TODO : Pourrait etre deplacer dans un autre singleton
    public void ReceiveAndroidMessage(string message)
    {
        GoogleReader.keyStr = message;
        Debug.Log(message);
    }

    /*

    private void FillReport(List<GoogleReader.Activity> activites, ref Report result)
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

            GoogleReader.Activity nowActivity = activites[i];
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

                GoogleReader.Activity firstActivity = activites[0];
                GoogleReader.Activity previousActivity = activites[i - 1];

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

    public List<GoogleReader.Activity> GetAllActiviesInTimeStamp(DateTime start, DateTime end, ExerciseType type = ExerciseType.Walk)
    {
        if (start.CompareTo(end) >= 1)
            return null;
        if (end.CompareTo(DateTime.Now) >= 1)
            return null;

        List<GoogleReader.Activity> result = activities;
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
                case GoogleReader.Activity.ActivityType.Walking:
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

    public GoogleReader.Activity GetLast()
    {
        if (activities.Count > 1)
            return activities.Last();
        return null;
    }
    */
}
