using System.Collections.Generic;
using UnityEngine;
 
using System;
using CCC.Persistence;

public class PendingReports : MonoPersistent
{
    private const string SAVE_KEY_ST = "pendingReports";

    [System.Serializable]
    public class PendingReport
    {
        //public ScheduledTask task;
        //public TimedTaskReport incompleteReport;
    }

    [SerializeField]
    private List<PendingReport> pendingReports = new List<PendingReport>();
    public bool log = true;
    public event SimpleEvent onReportConcluded;

    public static PendingReports instance;

    public override void Init(Action onComplete)
    {
        instance = this;
        ReadFromSaver();
        onComplete();

        PersistentLoader.LoadIfNotLoaded(CheckAndConcludeNextReport);
    }

    void CheckAndConcludeNextReport()
    {
        //if (pendingReports != null && pendingReports.Count > 0)
        //{
        //    PendingReport pending = pendingReports[0];
        //    ExerciseTrackingReport trackingReport = pending.incompleteReport.trackingReport;

        //    if (trackingReport != null && 
        //        (trackingReport.state == ExerciseTrackingReport.State.Completed
        //        || trackingReport.state == ExerciseTrackingReport.State.UserSaidItWasCompleted))
        //    {
        //        TaskConclusionWindow.ConcludeTask(pending.task, pending.incompleteReport,
        //            () =>
        //            {
        //                RemovePendingReport(pending);
        //            });
        //    }
        //    else
        //    {
        //        MessagePopup.DisplayMessage("Exercice non-compl\u00E9t\u00E9 planifi\u00E9 pour le "
        //            + pending.incompleteReport.taskPlannedFor.start.ToString() + ".");
        //        RemovePendingReport(pending);
        //    }
        //}
    }

    //public void AddPendingReport(ScheduledTask task, TimedTaskReport incompleteReport)
    //{
    //    if (pendingReports == null)
    //        pendingReports = new List<PendingReport>();
    //    pendingReports.Add(new PendingReport() { task = task, incompleteReport = incompleteReport });

    //    ApplyToSaver(true);

    //    if (log)
    //        Debug.Log("Ajout d'un nouveau 'pending report'.");

    //    CheckAndConcludeNextReport();
    //}

    private void RemovePendingReport(PendingReport report)
    {
        //if (pendingReports.Remove(report))
        //{
        //    ApplyToSaver(true);

        //    if (onReportConcluded != null)
        //        onReportConcluded();

        //    try
        //    {
        //        History.instance.AddTaskReport(report.incompleteReport);
        //    }
        //    catch (Exception e)
        //    {
        //        MessagePopup.DisplayMessage("Erreur lors de l'ajout du rapport dans l'history:\n " + e.Message);
        //    }
        //    if (log)
        //        Debug.Log("Retrait d'un 'pending report'.");
        //}
        //else
        //{
        //    Debug.LogWarning("On a essayer de retirer un 'pending report' qui n'etait pas dans la liste.");
        //}
        //CheckAndConcludeNextReport();
    }



    #region R/W Gamesaves
    private void ApplyToSaver(bool andSave)
    {
        var dataSaver = DataSaverBank.Instance.GetDataSaver(DataSaverBank.Type.Calendar);
        dataSaver.SetObjectClone(SAVE_KEY_ST, pendingReports);
        if (andSave)
        {
            //int c = pendingReports.Count;
            //print("Sauvgarde " + pendingReports.Count + " pending reports");
            dataSaver.SetDataDirty();
        }
    }

    private void ReadFromSaver()
    {
        var dataSaver = DataSaverBank.Instance.GetDataSaver(DataSaverBank.Type.Calendar);
        pendingReports = dataSaver.GetObjectClone(SAVE_KEY_ST) as List<PendingReport>;
        if (pendingReports == null)
            pendingReports = new List<PendingReport>();
    }
    #endregion
}
