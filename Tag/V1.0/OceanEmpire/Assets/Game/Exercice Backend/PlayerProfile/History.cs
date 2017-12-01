using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using System;

/// <summary>
/// On store tout !
/// </summary>
[System.Serializable]
public class History : CCC.Manager.BaseManager<History>
{
    private const string SAVEKEY_TaskReports = "taskReports";
    private const string SAVEKEY_PurchaseReports = "purchaseReports";

    [SerializeField]
    private List<TimedTaskReport> taskReports = new List<TimedTaskReport>();
    [SerializeField]
    private List<PurchaseReport> purchaseReport = new List<PurchaseReport>();

    public ReadOnlyCollection<TimedTaskReport> GetTaskReports()
    {
        return taskReports.AsReadOnly();
    }
    public ReadOnlyCollection<PurchaseReport> GetPurchaseReports()
    {
        return purchaseReport.AsReadOnly();
    }

    public override void Init()
    {
        Reload();
        CompleteInit();
    }

    public void AddTaskReport(TimedTaskReport report)
    {
        taskReports.Add(report);

        GameSaves.instance.SetObjectClone(GameSaves.Type.History, SAVEKEY_TaskReports, taskReports);
        Save();
    }
    public void AddPurchaseReport(PurchaseReport report)
    {
        purchaseReport.Add(report);

        GameSaves.instance.SetObjectClone(GameSaves.Type.History, SAVEKEY_PurchaseReports, purchaseReport);
        Save();
    }

    public string GetDataToString()
    {

        string completeText = "Task Reports:\n\n";
        for (int i = taskReports.Count - 1; i >= 0; i--)
        {
            completeText += taskReports[i].ToString() + "\n\n";
        }

        completeText += "\n\nPurchase Reports:\n\n";
        for (int i = purchaseReport.Count - 1; i >= 0; i--)
        {
            completeText += purchaseReport[i].ToString() + "\n\n";
        }

        return completeText;
    }

    #region R/W Gamesaves
    private void Save()
    {
        GameSaves.instance.SaveDataAsync(GameSaves.Type.History, null);
    }

    public void Reload()
    {
        taskReports = GameSaves.instance.GetObjectClone(GameSaves.Type.History, SAVEKEY_TaskReports) as List<TimedTaskReport>;
        if (taskReports == null)
            taskReports = new List<TimedTaskReport>();

        purchaseReport = GameSaves.instance.GetObjectClone(GameSaves.Type.History, SAVEKEY_PurchaseReports) as List<PurchaseReport>;
        if (purchaseReport == null)
            purchaseReport = new List<PurchaseReport>();
    }
    #endregion
}