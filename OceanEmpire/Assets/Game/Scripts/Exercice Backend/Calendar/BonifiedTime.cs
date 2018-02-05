using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BonifiedTime
{
    public TimeSlot timeslot;
    public float bonusStrength;

    public BonifiedTime(TimeSlot timeslot, float bonusStrength)
    {
        this.timeslot = timeslot;
        this.bonusStrength = bonusStrength;
    }
}
