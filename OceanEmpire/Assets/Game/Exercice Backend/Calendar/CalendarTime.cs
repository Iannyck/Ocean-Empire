using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TimeSlot
{
    public DateTime dateTime;
    public TimeSpan duration;

    public TimeSlot(DateTime dateTime, TimeSpan duration)
    {
        this.dateTime = dateTime;
        this.duration = duration;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is TimeSlot))
        {
            return false;
        }

        var objCT = (TimeSlot)obj;

        return objCT.dateTime == dateTime;
    }

    public override int GetHashCode()
    {
        return dateTime.GetHashCode();
    }

    public static bool operator ==(TimeSlot c1, TimeSlot c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(TimeSlot c1, TimeSlot c2)
    {
        return !c1.Equals(c2);
    }

    public bool IsInTheFuture()
    {
        return dateTime > DateTime.Now;
    }
}