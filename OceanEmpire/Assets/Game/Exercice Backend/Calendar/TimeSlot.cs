using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TimeSlot
{
    public DateTime start;
    public TimeSpan duration;
    public DateTime end { get { return start + duration; } }

    public TimeSlot(DateTime dateTime, TimeSpan duration)
    {
        this.start = dateTime;
        this.duration = duration;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is TimeSlot))
        {
            return false;
        }

        var objCT = (TimeSlot)obj;

        return objCT.start == start && objCT.duration == duration;
    }

    public override int GetHashCode()
    {
        return start.GetHashCode();
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
        return IsOverlappingWith(DateTime.Now) == 1;
    }

    public bool IsInThePast()
    {
        return IsOverlappingWith(DateTime.Now) == -1;
    }
    public bool IsNow()
    {
        return IsOverlappingWith(DateTime.Now) == 0;
    }

    public int IsOverlappingWith(TimeSlot timeslot)
    {
        DateTime end = this.end;
        DateTime otherEnd = timeslot.end;
        DateTime otherStart = timeslot.start;

        if (start < otherStart)
        {
            if (end < otherStart)
                return -1;
            else
                return 0;
        }
        else
        {
            if (start >= otherEnd)
                return 1;
            else
                return 0;
        }

    }
    public int IsOverlappingWith(DateTime dateTime)
    {
        if (start <= dateTime)
        {
            if (end >= dateTime)
                return 0;
            else
                return -1;
        }
        else
        {
            return 1;
        }
    }

    public override string ToString()
    {
        return "Start: " + start.ToString()
            + "\nEnd: " + end.ToString();
    }
}