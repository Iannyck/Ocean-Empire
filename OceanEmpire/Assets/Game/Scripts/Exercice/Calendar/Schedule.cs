using System;
using UnityEngine;

/// <summary>
/// Un plan de temps bonifié. Généralement stocké dans le calendrier
/// </summary>
[Serializable]
public class Schedule : IComparable
{
    public TimeSlot timeSlot;
    public DateTime createdOn;
    public Task task;
    public bool requiresConculsion;

    public Schedule(TimeSlot timeSlot)
    {
        this.timeSlot = timeSlot;
        createdOn = DateTime.Now;
    }

    /// <summary>
    /// Construit une nouvelle instance de temps bonifié.
    /// </summary>
    public BonifiedTime GetBonifiedTime() { return new BonifiedTime(timeSlot, new Bonus(1)); }


    public static TimeSpan DefaultDuration()
    {
        return new TimeSpan(1, 0, 0);
    }
    public static Bonus DefaultBonus()
    {
        return new Bonus(2);
    }

    public int CompareTo(object obj)
    {
        Schedule castedObj = obj as Schedule;
        if (castedObj != null)
        {
            return timeSlot.start.CompareTo(castedObj.timeSlot.start);
        }
        else
        {
            return -1;
        }
    }
}
