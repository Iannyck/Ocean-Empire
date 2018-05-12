
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Persistence;
using System.Text;

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

    [SerializeField] private DataSaver playerProfileSaver;

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
            //Debug.Log("ASKING FOR ACTIVITIES");
            if (activities != null)
            {
                //Debug.Log("GETTING ACTIVITIES");
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
        if (activities == null)
            return;
        records = new List<ActivityReport>();
        //Debug.Log("UNITY ACTIVITIES COUNT : " + activities.Count);
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
                else if (currentBest.rate < currentProb)
                {
                    currentBest.rate = currentProb;
                    currentBest.type = currentType;
                }
                else if (currentBest.rate == currentProb)
                {
                    int result = priority.Compare(currentBest.type, currentType);
                    if (result == 1)
                    {
                        currentBest.rate = currentProb;
                        currentBest.type = currentType;
                    }
                }
            }
            currentReport.best = currentBest;
            currentReport.time = currentActivity.time;

            records.Add(currentReport);
            //Debug.Log("ADDING REPORT : " + currentReport.best.type + "|" + currentReport.best.rate);
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
        Debug.Log("KEY RECEIVE : " + GoogleReader.keyStr);
    }

    public string GetAllData()
    {
        if (records == null)
            records = new List<ActivityReport>();

        StringBuilder text = new StringBuilder();

        text.Append("Date,WalkProb,RunProb,BicycleProb");
        for (int i = 1; i < (records.Count + 1); i++)
        {
            text.Append('\n')
                .Append(records[i - 1].time.ToString())
                .Append(',')
                .Append(records[i - 1].backupActivity.GetActivityProbability(PrioritySheet.ExerciseTypes.walk))
                .Append(',')
                .Append(records[i - 1].backupActivity.GetActivityProbability(PrioritySheet.ExerciseTypes.run))
                .Append(',')
                .Append(records[i - 1].backupActivity.GetActivityProbability(PrioritySheet.ExerciseTypes.bicycle));
        }

        return text.ToString();
    }
}
