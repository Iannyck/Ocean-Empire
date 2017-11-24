using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< .merge_file_a16704
public class InstantTask : TimedTask
=======
public class InstantTask : ScheduledTask
>>>>>>> .merge_file_a01164
{
    public override TaskReport BuildReport()
    {
        return new TaskReport(null, DateTime.Now, plannedOn.dateTime);
    }

    public InstantTask(Task task)
        :base(task, new CalendarTime(DateTime.Now))
    {
    }
}
