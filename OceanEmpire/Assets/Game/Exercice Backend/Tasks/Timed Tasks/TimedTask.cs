using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class TimedTask
{
    public Task task;
    public CalendarTime timeSlot;
    public DateTime createdOn;

    public abstract TimedTaskReport BuildReport();
    public abstract bool IsVisibleInCalendar();
}
