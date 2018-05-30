using CCC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlannedExerciceRewarder : MonoPersistent
{
    public static PlannedExerciceRewarder Instance { get; private set; }

    [Serializable]
    public class Report
    {
        public State state;
        public Schedule schedule;
        public float recordedExerciseVolume;
        public string playerConclusion = "null";

        public enum State { Ongoing, Completed, Failed }
        public float GetCompletionRate01() { return Mathf.Clamp01(recordedExerciseVolume / schedule.task.minDuration); }

        public override string ToString()
        {
            return "state(" + state +
                ") recordedVolume(" + recordedExerciseVolume +
                ") conclusion(" + playerConclusion +
                ") schedule(" + schedule;
        }
    }

    public Report LatestPendingReport { get; private set; }
    public event Action OnLatestPendingReportUpdated;
    public event Action<Report> OnReportFinalized;

    [SerializeField] float analysisCooldown = 5.1f;
    [SerializeField] SceneInfo failureWindow;
    [SerializeField] SceneInfo completionWindow;
    [SerializeField] AnalyserGroup analyserGroup;
    [SerializeField] DataSaver dataSaver;
    [SerializeField] ScriptableActionQueue shackAnimQueue;

    float analysisTimer = 0;
    List<Report> previousReports;
    public ReadOnlyCollection<Report> GetPreviousReports() { return previousReports.AsReadOnly(); }

    public override void Init(Action onComplete)
    {
        Instance = this;

        dataSaver.OnReassignData += FetchData;

        if (dataSaver.HasEverLoaded)
        {
            FetchData();
            onComplete();
        }
        else
        {
            dataSaver.LateLoad(onComplete);
        }
    }

    void Update()
    {
        if (analysisTimer <= 0)
        {
            CheckAnalyse();
            analysisTimer = analysisCooldown;
        }
        analysisTimer -= Time.unscaledDeltaTime;
    }

    public bool CanOverwriteThePendingReport
    {
        get
        {
            return LatestPendingReport == null || LatestPendingReport.state == Report.State.Ongoing;
        }
    }

    #region Analyse
    public void ForceAnalyseCheck() { CheckAnalyse(); }
    void CheckAnalyse()
    {
        if (CanOverwriteThePendingReport)
            Analyse();
    }
    void Analyse()
    {
        if (!CanOverwriteThePendingReport)
        {
            Debug.LogError("Should not be analysing because we already have a " +
                "pending report in this state: " + LatestPendingReport.state);
        }

        if (!Scenes.IsActive("Shack"))
            return;

        // Regardons s'il n'y a pas un Schedule du passé qui nécéssiterait encore une analyse
        var past = Calendar.instance.GetPastSchedules();
        for (int i = 0; i < past.Count; i++)
        {
            if (past[i].requiresConculsion)
            {
                LatestPendingReport = Analyse(past[i]);
                if (LatestPendingReport != null)
                    break;
                else
                {
                    Debug.LogError("This should never happen");
                    past[i].requiresConculsion = false; // NE DEVRAIS JAMAIS ARRIVER
                }
            }
        }

        // Si nous n'avons pas de nouveau Rapport, regardons si nous avons un exercice en-cours
        if (CanOverwriteThePendingReport)
        {
            var presentAndFuture = Calendar.instance.GetPresentAndFutureSchedules();
            if (presentAndFuture.Count > 0 && presentAndFuture[0].requiresConculsion)
            {
                var now = DateTime.Now;
                if (presentAndFuture[0].timeSlot.start <= now)
                {
                    LatestPendingReport = Analyse(presentAndFuture[0], now);
                }
            }
        }

        if (OnLatestPendingReportUpdated != null)
            OnLatestPendingReportUpdated();



        // Avons nous un rapport à conclure ?
        if (LatestPendingReport != null && LatestPendingReport.state != Report.State.Ongoing)
        {
            switch (LatestPendingReport.state)
            {
                case Report.State.Completed:
                    {
                        // Congrats window !
                        Action onAnimComplete = null;
                        Shack_Canvas shackCanvases = Scenes.IsActive("Shack") ?
                            Scenes.GetActive("Shack").FindRootObject<Shack_Canvas>() :
                            null;
                        shackAnimQueue.ActionQueue.AddAction(() =>
                        {
                            if (shackCanvases)
                                shackCanvases.HideAll();
                            Scenes.Load(completionWindow, (scene) =>
                            {
                                scene.FindRootObject<CompletionWindow>().FillContent(LatestPendingReport, () =>
                                {
                                    if (shackCanvases)
                                        shackCanvases.ShowAll();
                                    onAnimComplete();
                                    FinalizeLatestReport();
                                });
                            });
                        }, 2, out onAnimComplete);
                        break;
                    }
                case Report.State.Failed:
                    {
                        // Failure window
                        Action onAnimComplete = null;
                        Shack_Canvas shackCanvases = Scenes.IsActive("Shack") ?
                            Scenes.GetActive("Shack").FindRootObject<Shack_Canvas>() :
                            null;
                        shackAnimQueue.ActionQueue.AddAction(() =>
                        {
                            if (shackCanvases)
                                shackCanvases.HideAll();
                            Scenes.Load(failureWindow, (scene) =>
                            {
                                scene.FindRootObject<FailureAsk>().FillContent(LatestPendingReport, () =>
                                {
                                    if (shackCanvases)
                                        shackCanvases.ShowAll();
                                    onAnimComplete();
                                    FinalizeLatestReport();
                                });
                            });
                        }, 2, out onAnimComplete);
                        break;
                    }
            }
        }
    }
    Report Analyse(Schedule schedule) { return Analyse(schedule, DateTime.Now); }
    Report Analyse(Schedule schedule, DateTime now)
    {
        if (schedule.timeSlot.start > now)
        {
            // L'exercice est dans le futur
            return null;
        }


        //---------DÉBUT DE L'ANALYSE---------//

        var report = new Report()
        {
            schedule = schedule
        };

        // Ça nous sert à rien d'analyser le futur
        var timeSlotToAnalyse = schedule.timeSlot;
        if (timeSlotToAnalyse.end > now)
            timeSlotToAnalyse.end = now;

        // Analyse Group Report
        var analyserGroupReport = analyserGroup.GetExerciseVolume(timeSlotToAnalyse);

        // Sum up the exercise volume
        float recordedExerciseVolume = 0;
        foreach (AnalyserReport individualReport in analyserGroupReport.individualReports)
        {
            recordedExerciseVolume += individualReport.volume.volume;
        }

        report.recordedExerciseVolume = recordedExerciseVolume;


        //---------Verdict---------//

        bool forceCalendarUpdate = false;
        if (recordedExerciseVolume >= schedule.task.minDuration)
        {
            report.state = Report.State.Completed;
            forceCalendarUpdate = true;
        }
        else
        {
            if (schedule.timeSlot.end > now)
            {
                report.state = Report.State.Ongoing;
            }
            else
            {
                report.state = Report.State.Failed;
                forceCalendarUpdate = true;
            }
        }

        // Truncate the schedule's timeslot early, if necessary
        if (report.state != Report.State.Ongoing && schedule.timeSlot.end > now)
            schedule.timeSlot.end = now - new TimeSpan(1);

        if (forceCalendarUpdate)
            Calendar.instance.ForceSchedulesUpdate();

        return report;
    }
    #endregion

    void FinalizeLatestReport()
    {
        Debug.Log("Finalizing report...");
        // Reward player
        if (LatestPendingReport.state == Report.State.Completed)
            PlayerCurrency.AddTickets(LatestPendingReport.schedule.task.ticketReward);

        // Mark schedule as concluded
        LatestPendingReport.schedule.requiresConculsion = false;
        Calendar.instance.ForceSave();

        var report = LatestPendingReport;

        // Add report to archive
        previousReports.Add(LatestPendingReport);

        // Remove pending report
        LatestPendingReport = null;

        // Raise event
        if (OnLatestPendingReportUpdated != null)
            OnLatestPendingReportUpdated();

        if (OnReportFinalized != null)
            OnReportFinalized(report);

        Logger.Log(Logger.Category.PlannedExercise, "Finalized: " + report.ToString());

        // Save
        Save();
        Debug.Log("Report finalized");

        if (Application.isMobilePlatform)
            GoogleActivities.instance.ClearAllActivitiesSave();
    }


    #region Load/Save
    private const string PREVIOUS_REPORTS_KEY = "previousRep";
    void FetchData()
    {
        var obj = dataSaver.GetObjectClone(PREVIOUS_REPORTS_KEY);
        if (obj != null)
            previousReports = (List<Report>)obj;
        else
            previousReports = new List<Report>();
    }

    void Save()
    {
        dataSaver.SetObjectClone(PREVIOUS_REPORTS_KEY, previousReports);
        dataSaver.LateSave();
    }
    #endregion
}
