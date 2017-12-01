using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class TimedTask
{
    public Task task;
    public TimeSlot timeSlot;
    public DateTime createdOn;

    public abstract bool IsVisibleInCalendar();
}
