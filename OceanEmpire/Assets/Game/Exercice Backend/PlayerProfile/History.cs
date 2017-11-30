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
    private const string SAVEKEY_reports = "taskReports";

    [SerializeField]
    private List<TimedTaskReport> taskReports = new List<TimedTaskReport>();

    public ReadOnlyCollection<TimedTaskReport> GetTaskReports()
    {
        return taskReports.AsReadOnly();
    }
    public override void Init()
    {
        ReadFromGameSaves();
        CompleteInit();
    }
    public void AddReport(TimedTaskReport report)
    { 
        taskReports.Add(report);
        ApplyToGameSaves(true);
    }

    public void Reload()
    {
        ReadFromGameSaves();
    }

    #region R/W Gamesaves
    private void ApplyToGameSaves(bool andSave)
    {
        GameSaves.instance.SetObjectClone(GameSaves.Type.History, SAVEKEY_reports, taskReports);
        if (andSave)
            GameSaves.instance.SaveDataAsync(GameSaves.Type.History, null);
    }

    private void ReadFromGameSaves()
    {
        taskReports = GameSaves.instance.GetObjectClone(GameSaves.Type.History, SAVEKEY_reports) as List<TimedTaskReport>;
        if (taskReports == null)
            taskReports = new List<TimedTaskReport>();
    }
    #endregion
}