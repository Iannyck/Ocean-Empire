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
    private List<TaskReport> taskReports = new List<TaskReport>();

    public ReadOnlyCollection<TaskReport> GetTaskReports()
    {
        return taskReports.AsReadOnly();
    }
    public void AddReport(TaskReport report)
    { 
        taskReports.Add(report);
    }
    public void ClearTaskReports()
    {
        taskReports.Clear();
    }
}