using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TimeSlot
{
    public DateTime start;
    public TimeSpan duration { get { return end - start; } }
    public DateTime end;

    public TimeSlot(DateTime dateTime, TimeSpan duration)
    {
        start = dateTime;
        end = start + duration;
    }

    public TimeSlot(TimeSlot copy)
    {
        start = copy.start;
        end = copy.end;
    }

    public TimeSlot(DateTime start, DateTime end)
    {
        this.start = start;
        this.end = end;
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
    /// <para/>-1 = instance -> other
    /// <para/>0 = overlap
    /// <para/>1 = other -> instance
    /// </summary>
    public int IsOverlappingWith(TimeSlot other)
    {
        if (start < other.start)
        {
            if (end > other.start)
                return 0;
            else
                return -1;
        }
        else
        {
            if (start < other.end)
                return 0;
            else
                return 1;
        }

    }

    /// <summary>
    /// La valeur d'overlap. 
    /// <para/>-1 = instance -> other
    /// <para/>0 = overlap
    /// <para/>1 = other -> instance
    /// <para/>overlappingDuration = la durée total de l'overlap
    /// </summary>
    public int IsOverlappingWith(TimeSlot other, out TimeSpan overlappingDuration)
    {
        TimeSlot t;
        var result = IsOverlappingWith(other, out t);
        overlappingDuration = t.duration;
        return result;
    }

    /// <summary>
    /// La valeur d'overlap. 
    /// <para/>-1 = instance -> other
    /// <para/>0 = overlap
    /// <para/>1 = other -> instance
    /// <para/>overlappingTimeslot = la time slot formé par l'overlap
    /// </summary>
    public int IsOverlappingWith(TimeSlot other, out TimeSlot overlappingTimeslot)
    {
        if (start < other.start)
        {
            if (end > other.start)
            {
                if (end > other.end)
                    overlappingTimeslot = other;
                else
                    overlappingTimeslot = new TimeSlot(other.start, end);
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
            if (start < other.end)
            {
                if (other.end < end)
                    overlappingTimeslot = new TimeSlot(start, other.end);
                else
                    overlappingTimeslot = this;
                return 0;
            }
            else
            {
                overlappingTimeslot = new TimeSlot(new DateTime(), new DateTime());
                return 1;
            }
        }

    }

    /// <summary>
    /// La valeur d'overlap.
    /// <para/>-1 = instance -> date
    /// <para/>0 = overlap
    /// <para/>1 = date -> instance
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
            + " - End: " + end.ToString();
    }

    public string ToCondensedDayOfTimeString()
    {
        return start.ToCondensedTimeOfDayString() + " - " + end.ToCondensedTimeOfDayString();
    }
}