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

    public TimeSlot(TimeSlot copy)
    {
        start = copy.start;
        duration = copy.duration;
    }

    public TimeSlot(DateTime start, DateTime end)
    {
        this.start = start;
        duration = end - start;
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

    /// <summary>
    /// La valeur d'overlap. 
    /// <para/>-1 = instance -> timeslot en paramètre
    /// <para/>0 = overlap
    /// <para/>1 = timeslot en paramètre -> instance
    /// </summary>
    public int IsOverlappingWith(TimeSlot timeslot)
    {
        DateTime end = this.end;
        DateTime otherEnd = timeslot.end;
        DateTime otherStart = timeslot.start;

        if (start < otherStart)
        {
            if (end > otherStart)
                return 0;
            else
                return -1;
        }
        else
        {
            if (start < otherEnd)
                return 0;
            else
                return 1;
        }

    }

    /// <summary>
    /// La valeur d'overlap. 
    /// <para/>-1 = instance -> timeslot en paramètre
    /// <para/>0 = overlap
    /// <para/>1 = timeslot en paramètre -> instance
    /// <para/>overlappingDuration = la durée total de l'overlap
    /// </summary>
    public int IsOverlappingWith(TimeSlot timeslot, out TimeSpan overlappingDuration)
    {
        TimeSlot t;
        var result = IsOverlappingWith(timeslot, out t);
        overlappingDuration = t.duration;
        return result;
    }

    /// <summary>
    /// La valeur d'overlap. 
    /// <para/>-1 = instance -> timeslot en paramètre
    /// <para/>0 = overlap
    /// <para/>1 = timeslot en paramètre -> instance
    /// <para/>overlappingTimeslot = la timeslot formé par l'overlap
    /// </summary>
    public int IsOverlappingWith(TimeSlot timeslot, out TimeSlot overlappingTimeslot)
    {
        DateTime end = this.end;
        DateTime otherEnd = timeslot.end;
        DateTime otherStart = timeslot.start;

        if (start < otherStart)
        {
            if (end > otherStart)
            {
                if (end > otherEnd)
                    overlappingTimeslot = timeslot;
                else
                    overlappingTimeslot = new TimeSlot(otherStart, end);
                return 0;
            }
            else
            {
                overlappingTimeslot = new TimeSlot(new DateTime(), new TimeSpan(0));
                return -1;
            }
        }
        else
        {
            if (start < otherEnd)
            {
                if (otherEnd < end)
                    overlappingTimeslot = new TimeSlot(start, otherEnd);
                else
                    overlappingTimeslot = this;
                return 0;
            }
            else
            {
                overlappingTimeslot = new TimeSlot(new DateTime(), new TimeSpan(0));
                return 1;
            }
        }

    }

    /// <summary>
    /// La valeur d'overlap.
    /// <para/>-1 = instance -> date en paramètre
    /// <para/>0 = overlap
    /// <para/>1 = date en paramètre -> instance
    /// </summary>
    public int IsOverlappingWith(DateTime dateTime)
    {
        if (start < dateTime)
        {
            if (end > dateTime)
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

    public string ToCondensedDayOfTimeString()
    {
        return start.ToCondensedDayOfTimeString() + " - " + end.ToCondensedDayOfTimeString();
    }
}