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
        public int dayOfMonth;
        public DayOfWeek dayOfWeek;
        public int monthOfYear;
        public int year;

        public Day(DateTime around)
        {
            dayOfMonth = around.Day;
            dayOfWeek = around.DayOfWeek;
            monthOfYear = around.Month;
            year = around.Year;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Day))
            {
                return false;
            }

            var d = (Day)obj;
            return d.dayOfMonth == dayOfMonth && d.monthOfYear == monthOfYear && d.year == year;
        }
        public override int GetHashCode()
        {
            return dayOfMonth + monthOfYear + year;
        }
    }
    public class Week
    {
        public Day[] days;
        public Week(DateTime around)
        {
            days = new Day[7];
            FillDaysData(around);
        }
        public Week(Week week, int weeksOffset)
        {
            days = new Day[7];
            FillDaysData(week.GetDateTimeAnchor().AddWeeks(weeksOffset));
        }

        public DateTime GetDateTimeAnchor()
        {
            return new DateTime(days[0].year, days[0].monthOfYear, days[0].dayOfMonth);
        }

        private void FillDaysData(DateTime around)
        {
            int index = (int)around.DayOfWeek;
            for (int i = 0; i < days.Length; i++)
            {
                days[i] = new Day(around.AddDays(i - index));
            }
        }
    }

    /// <summary>
    /// Retourne 4 semaine
    /// </summary>
    /// <param name="time">La date qui servira a identifier la semaine de depart</param>
    /// <param name="offset">Offset de semaine. + = futur   - = pass�</param>
    public static List<Week> GetFiveWeeksFrom(DateTime time, int offset = 0)
    {
        List<Week> weeks = new List<Week>(4)
        {
            GetWeekAround(time, offset),
            GetWeekAround(time, 1 + offset),
            GetWeekAround(time, 2 + offset),
            GetWeekAround(time, 3 + offset)
        };
        return weeks;
    }

    /// <summary>
    /// Retourne les meme 5 semaine que le calendrier Windows
    /// </summary>
    public static List<Week> GetFiveWeeksOfTheMonth(DateTime time)
    {
        return GetFiveWeeksOfTheMonth(time.Year, time.Month);
    }

    /// <summary>
    /// Retourne les meme 5 semaine que le calendrier Windows
    /// </summary>
    public static List<Week> GetFiveWeeksOfTheMonth(int year, int month)
    {
        DateTime firstDay = new DateTime(year, month, 1);
        return GetFiveWeeksFrom(firstDay, 0);
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
