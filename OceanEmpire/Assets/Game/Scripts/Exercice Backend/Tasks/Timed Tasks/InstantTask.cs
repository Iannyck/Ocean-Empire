using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InstantTask : ScheduledTask
{
    public InstantTask(Task task)
        :base(task, DateTime.Now)
    {
    }

    public override string ToString()
    {
        return "INSTANT " + base.ToString();
    }
}
