using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using System;
using CCC.Persistence;

/// <summary>
/// On store tout !
/// </summary>
[System.Serializable]
public class History : MonoPersistent
{
    private const string SAVEKEY_TaskReports = "taskReports";
    private const string SAVEKEY_PurchaseReports = "purchaseReports";

    [SerializeField]
    private DataSaver dataSaver;

    //[SerializeField]
    //private List<TimedTaskReport> taskReports = new List<TimedTaskReport>();
    [SerializeField]
    private List<PurchaseReport> purchaseReport = new List<PurchaseReport>();

    public static History instance;

    public override void Init(Action onComplete)
    {
        instance = this;
        FetchData();
        onComplete();
    }
    protected void Awake()
    {
        dataSaver.OnReassignData += FetchData;
    }

    //public ReadOnlyCollection<TimedTaskReport> GetTaskReports()
    //{
    //    return taskReports.AsReadOnly();
    //}
    public ReadOnlyCollection<PurchaseReport> GetPurchaseReports()
    {
        return purchaseReport.AsReadOnly();
    }

    //public void AddTaskReport(TimedTaskReport report)
    //{
    //    taskReports.Add(report);

    //    dataSaver.SetObjectClone(SAVEKEY_TaskReports, taskReports);
    //    Save();
    //}
    public void AddPurchaseReport(PurchaseReport report)
    {
        purchaseReport.Add(report);

        dataSaver.SetObjectClone(SAVEKEY_PurchaseReports, purchaseReport);
        Save();
    }

    public string GetDataToString()
    {

        string completeText = "Task Reports:\n\n";
        //for (int i = taskReports.Count - 1; i >= 0; i--)
        //{
        //    completeText += taskReports[i].ToString() + "\n\n";
        //}

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
        dataSaver.SaveAsync();
    }

    private void FetchData()
    {
        //taskReports = dataSaver.GetObjectClone(SAVEKEY_TaskReports) as List<TimedTaskReport>;
        //if (taskReports == null)
        //    taskReports = new List<TimedTaskReport>();

        purchaseReport = dataSaver.GetObjectClone(SAVEKEY_PurchaseReports) as List<PurchaseReport>;
        if (purchaseReport == null)
            purchaseReport = new List<PurchaseReport>();
    }
    #endregion
}