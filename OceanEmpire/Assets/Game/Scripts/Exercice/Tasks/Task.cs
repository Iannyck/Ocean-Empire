using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Task
{
    public int level;
    public float minDuration;
    public float advertisedDuration;
    public float maxDuration;
    public int ticketReward;

    public TimeSpan GetMaxDurationAsTimeSpan()
    {
        return new TimeSpan(0, 0, Mathf.RoundToInt(maxDuration * 60));
    }
    public TimeSpan GetMinDurationAsTimeSpan()
    {
        return new TimeSpan(0, 0, Mathf.RoundToInt(minDuration * 60));
    }

    public override string ToString()
    {
        return "Task: volumeRequired(" + minDuration + ")  maxDuration(" + maxDuration + ")  ticketReward(" + ticketReward + ")";
    }
}