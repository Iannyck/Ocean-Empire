using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CalendarTime
{
    public DateTime dateTime;
    //Autre chose, si necessaire

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
}