using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduledTask : ITaskReportBuilder
{
    public Task task;
    public CalendarTime time;

    public TaskReport BuildReport()
    {
        throw new NotImplementedException();
    }
}
