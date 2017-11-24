using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class TimedTask
{
    public Task task;
    public CalendarTime plannedOn;

    public abstract TaskReport BuildReport();
    public abstract bool IsVisibleInCalendar();
}
