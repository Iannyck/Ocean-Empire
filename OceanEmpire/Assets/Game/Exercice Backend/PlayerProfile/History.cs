using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

/// <summary>
/// On store tout !
/// </summary>
[System.Serializable]
public class History
{
    private List<TimedTaskReport> taskReports = new List<TimedTaskReport>();

    public ReadOnlyCollection<TimedTaskReport> GetTaskReports()
    {
        return taskReports.AsReadOnly();
    }
    public void AddReport(TimedTaskReport report)
    { 
        taskReports.Add(report);
    }
    public void ClearTaskReports()
    {
        taskReports.Clear();
    }
}