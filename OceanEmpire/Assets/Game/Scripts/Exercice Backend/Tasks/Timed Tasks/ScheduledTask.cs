using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScheduledTask : TimedTask
{
    public virtual TimeSpan GetTotalAllocatedTime()
    {
        return new TimeSpan(0, 15, 0);
    }

    public ScheduledTask(Task task, DateTime plannedOn)
    {
        this.task = task;
        createdOn = DateTime.Now;
        timeSlot = new TimeSlot(plannedOn, task.GetAllocatedTime());
    }
    
    public override bool IsVisibleInCalendar()
    {
        return true;
    }

    public override string ToString()
    {
        return "Task:\n" + task.ToString() + "\n\n"
                + "Timeslot:\n" + timeSlot.ToString() + "\n\n"
                + "Created On:\n" + createdOn.ToString();
    }

    public bool SuccesfullyCompleted()
    {
        Reward reward = task.GetReward();
        bool applyResult = reward.Apply();

        PlayerProfile.UpdatePlayerLevel(task);
        return applyResult;
    }
}
