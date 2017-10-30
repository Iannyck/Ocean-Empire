using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CalendarTime
{
    public DateTime dateTime;
    public CalendarTime(DateTime dateTime)
    {
        this.dateTime = dateTime;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is CalendarTime))
        {
            return false;
        }

        var objCT = (CalendarTime)obj;

        return objCT.dateTime == dateTime;
    }

    public override int GetHashCode()
    {
        return dateTime.GetHashCode();
    }

    public static bool operator ==(CalendarTime c1, CalendarTime c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(CalendarTime c1, CalendarTime c2)
    {
        return !c1.Equals(c2);
    }

    public bool IsInTheFuture()
    {
        return dateTime > DateTime.Now;
    }
}