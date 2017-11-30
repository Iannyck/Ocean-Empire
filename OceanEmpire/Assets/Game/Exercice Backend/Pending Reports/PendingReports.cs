using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using System.Collections.ObjectModel;
using System;

public class PendingReports : BaseManager<PendingReports>
{
    private const string SAVE_KEY_ST = "pendingReports";

    [System.Serializable]
    public class PendingReport
    {
        public ScheduledTask task;
        public TimedTaskReport incompleteReport;
    }

    [SerializeField]
    private List<PendingReport> pendingReports = new List<PendingReport>();
    public bool log = true;

    public override void Init()
    {
        ReadFromGameSaves();
        CompleteInit();

        MasterManager.Sync(CheckAndConcludeNextReport);
    }

    void CheckAndConcludeNextReport()
    {
        if (pendingReports != null && pendingReports.Count > 0)
        {
            PendingReport pending = pendingReports[0];
            ExerciseTrackingReport trackingReport = pending.incompleteReport.trackingReport;

            if (trackingReport != null && 
                (trackingReport.state == ExerciseTrackingReport.State.Completed
                || trackingReport.state == ExerciseTrackingReport.State.UserSaidItWasCompleted))
            {
                TaskConclusionWindow.ConcludeTask(pending.task, pending.incompleteReport,
                    () =>
                    {
                        RemovePendingReport(pending);
                    });
            }
            else
            {
                MessagePopup.DisplayMessage("Exercice non-compl\u00E9t\u00E9 planifi\u00E9 pour le "
                    + pending.incompleteReport.taskPlannedFor.start.ToString() + ".");
                RemovePendingReport(pending);
            }
        }
    }

    public void AddPendingReport(ScheduledTask task, TimedTaskReport incompleteReport)
    {
        if (pendingReports == null)
            pendingReports = new List<PendingReport>();
        pendingReports.Add(new PendingReport() { task = task, incompleteReport = incompleteReport });

        ApplyToGameSaves(true);

        if (log)
            Debug.Log("Ajout d'un nouveau 'pending report'.");

        CheckAndConcludeNextReport();
    }

    private void RemovePendingReport(PendingReport report)
    {
        if (pendingReports.Remove(report))
        {
            ApplyToGameSaves(true);
            Debug.LogWarning("ON DOIT L'AJOUTER A L'HISTORIQUE");

            if (log)
                Debug.Log("Retrait d'un 'pending report'.");
        }
        else
        {
            Debug.LogWarning("On a essayer de retirer un 'pending report' qui n'etait pas dans la liste.");
        }
        CheckAndConcludeNextReport();
    }



    #region R/W Gamesaves
    private void ApplyToGameSaves(bool andSave)
    {
        GameSaves.instance.SetObject(GameSaves.Type.Calendar, SAVE_KEY_ST, pendingReports);
        if (andSave)
            GameSaves.instance.SaveDataAsync(GameSaves.Type.Calendar, null);
    }

    private void ReadFromGameSaves()
    {
        pendingReports = GameSaves.instance.GetObject(GameSaves.Type.Calendar, SAVE_KEY_ST) as List<PendingReport>;
        if (pendingReports == null)
            pendingReports = new List<PendingReport>();
    }
    #endregion
}
