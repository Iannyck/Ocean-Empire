using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BonifiedTime : IComparable
{
    public TimeSlot timeslot;
    public float bonusStrength;

    public BonifiedTime(TimeSlot timeslot, float bonusStrength)
    {
        this.timeslot = timeslot;
        this.bonusStrength = bonusStrength;
    }

    public static TimeSpan DefaultDuration
    {
        get { return new TimeSpan(1,0,0); }
    }
    public static float DefaultStrength
    {
        get { return 2; }
    }

    public int CompareTo(object obj)
    {
        BonifiedTime castedObj = obj as BonifiedTime;
        if(castedObj != null)
        {
            return timeslot.start.CompareTo(castedObj.timeslot.start);
        }
        else
        {
            return -1;
        }
    }
}
