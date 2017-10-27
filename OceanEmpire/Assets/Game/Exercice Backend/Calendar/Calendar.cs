using System.Collections;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

public class Calendar
{
    private List<ScheduledTask> scheduledTasks = new List<ScheduledTask>();

    public ReadOnlyCollection<ScheduledTask> GetScheduledTasks() { return scheduledTasks.AsReadOnly(); }

    public bool AddScheduledTask(ScheduledTask task)
    {
        if (task.plannedOn.IsInTheFuture())
        {
            scheduledTasks.SortedAdd(task, (a, b) => a.plannedOn.dateTime.CompareTo(a.plannedOn.dateTime));
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool RemoveScheduledTask(ScheduledTask task)
    {
        return scheduledTasks.Remove(task);
    }

    public struct Day
    {
        public int number;
        public DayOfWeek dayOfWeek;
        public int monthOfYear;
        public int year;

        public override bool Equals(object obj)
        {
            if (!(obj is Day))
            {
                return false;
            }

            var d = (Day)obj;
            return d.number == number && d.monthOfYear == monthOfYear && d.year == year;
        }
        public override int GetHashCode()
        {
            return number + monthOfYear + year;
        }
    }
    public class Week
    {
        public Day[] days;
        public Week(DateTime around)
        {

        }
        public Week(Week week, int weeksOffset)
        {

        }
    }

    public static List<Week> GetFourWeeksAround(DateTime time)
    {
        List<Week> weeks = new List<Week>(4)
        {
            GetWeekAround(time, -1),
            GetWeekAround(time),
            GetWeekAround(time, 1),
            GetWeekAround(time, 2)
        };
        return weeks;
    }

    public static Week GetWeekAround(DateTime time)
    {
        return new Week(time);
    }

    public static Week GetWeekAround(DateTime time, int weekOffset)
    {
        if (weekOffset == 0)
            return GetWeekAround(time);
        return new Week(time.AddWeeks(weekOffset));
    }
}
