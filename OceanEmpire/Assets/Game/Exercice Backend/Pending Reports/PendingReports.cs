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
    public event SimpleEvent onPendingReportAdded;
    public event SimpleEvent onPendingReportRemoved;

    public override void Init()
    {
        ReadFromGameSaves();
        CompleteInit();
    }

    public void AddPendingReport(ScheduledTask task, TimedTaskReport incompleteReport)
    {
        pendingReports.Add(new PendingReport() { task = task, incompleteReport = incompleteReport });

        ApplyToGameSaves(true);

        if (log)
            Debug.Log("Ajout d'un nouveau 'pending report'.");

        if (onPendingReportAdded != null)
            onPendingReportAdded();
    }

    private void RemovePendingReport(PendingReport report)
    {
        if (pendingReports.Remove(report))
        {
            ApplyToGameSaves(true);
            Debug.LogWarning("ON DOIT L'AJOUTER A L'HISTORIQUE");

            if (log)
                Debug.Log("Retrait d'un 'pending report'.");

            if (onPendingReportRemoved != null)
                onPendingReportRemoved();
        }
        else
        {
            Debug.LogWarning("On a essayer de retirer un 'pending report' qui n'etait pas dans la liste.");
        }
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
