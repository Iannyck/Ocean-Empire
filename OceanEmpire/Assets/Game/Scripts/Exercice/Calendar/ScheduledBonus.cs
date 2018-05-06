using System;
using UnityEngine;

/// <summary>
/// Un plan de temps bonifié. Généralement stocké dans le calendrier
/// </summary>
[Serializable]
public class ScheduledBonus : IComparable
{
    public TimeSlot timeSlot;
    public DateTime createdOn;
    public Bonus bonus;
    public Task task;
    public bool displayPadding;
    public float minutesOfPadding;

    public ScheduledBonus(TimeSlot timeSlot, Bonus bonus)
    {
        this.timeSlot = timeSlot;
        this.bonus = bonus;
        createdOn = DateTime.Now;
    }

    /// <summary>
    /// Construit une nouvelle instance de temps bonifié.
    /// </summary>
    public BonifiedTime GetBonifiedTime() { return new BonifiedTime(timeSlot, bonus); }

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
        ScheduledBonus castedObj = obj as ScheduledBonus;
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
