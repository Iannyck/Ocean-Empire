using System.Collections;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using CCC.Manager;
using UnityEngine;

public class Calendar : BaseManager<Calendar>
{
    private const string CALENDAR_FILE = "calendar.dat";

    /// <summary>
    /// Ordonner du plus vieux au plus recent
    /// </summary>
    [SerializeField]
    private List<ScheduledTask> scheduledTasks = new List<ScheduledTask>();
    public bool log = true;

    public ReadOnlyCollection<ScheduledTask> GetScheduledTasks() { return scheduledTasks.AsReadOnly(); }

    public bool AddScheduledTask(ScheduledTask task)
    {
        if (IsTimeSlutAvailable(task.timeSlot))
        {
            scheduledTasks.SortedAdd(task, (a, b) => a.timeSlot.start.CompareTo(b.timeSlot.start));
            if (log)
                Debug.Log("ScheduledTask ajouter au calendrier avec succes.");
            return true;
        }
        else
        {
            if (log)
                Debug.LogWarning("La ScheduledTask n'a pas pu être ajouter au calendrier. " +
                    "La Timeslot est deja utilisé.");
            return false;
        }
    }

    /// <summary>
    /// ON NE DEVRAIS GÉNÉRALLEMENT PAS PASSSER PAR ICI
    /// </summary>
    public bool RemoveScheduledTask(ScheduledTask task)
    {
        Debug.LogWarning("Attention, on ne devrais generalement pas passer par ici pour conclure une scheduled task");
        return scheduledTasks.Remove(task);
    }

    public bool ConcludeScheduledTask(ScheduledTask task, TimedTaskReport report)
    {
        if (scheduledTasks.Remove(task))
        {
            Debug.LogWarning("A FAIRE: Ajouter le report a l'historique");

            return true;
        }
        return false;
    }

    /// <summary>
    /// ( ͡° ͜ʖ ͡°)
    /// </summary>
    public bool IsTimeSlutAvailable(TimeSlot timeslot)
    {
        for (int i = 0; i < scheduledTasks.Count; i++)
        {
            int result = timeslot.IsOverlappingWith(ref scheduledTasks[i].timeSlot);
            if (result == 0)
                return false;
            else if (result == -1)
                return true;
        }

        return true;
    }

    public override void Init()
    {
        CompleteInit();
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
        public DateTime GetAnchorDateTime() { return new DateTime(year, monthOfYear, dayOfMonth); }

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
        public static bool operator ==(Day a, Day b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Day a, Day b)
        {
            return !(a == b);
        }
    }

    public static List<Day> GetDaysFrom(Day startingDay, int numberOfDays)
    {
        return GetDaysFrom(startingDay.GetAnchorDateTime(), numberOfDays);
    }
    public static List<Day> GetDaysFrom(DateTime startingDay, int numberOfDays)
    {
        List<Day> days = new List<Day>(numberOfDays);
        for (int i = 0; i < numberOfDays; i++)
        {
            days.Add(new Day(startingDay.AddDays(i)));
        }
        return days;
    }

    public static List<Day> GetWeeksFrom(DateTime dayOfFirstWeek, int numberOfWeeks, int weekOffset)
    {
        int numberOfDays = numberOfWeeks * 7;
        List<Day> days = new List<Day>(numberOfDays);
        DateTime firstDay = GetDayOfWeek(dayOfFirstWeek.AddDays(weekOffset * 7), DayOfWeek.Sunday);

        for (int i = 0; i < numberOfDays; i++)
        {
            days.Add(new Day(firstDay.AddDays(i)));
        }
        return days;
    }
    public static List<Day> GetDaysOfMonth(DateTime someDayOfTheMonth, int numberOfDays, int monthOffset = 0)
    {
        DateTime firstDayOfMonth = GetDayOfMonth(someDayOfTheMonth.AddMonths(monthOffset), 1);
        return GetDaysFrom(firstDayOfMonth, numberOfDays);
    }

    public static List<Day> GetWeeksOfMonth(DateTime someDayOfTheMonth, int numberOfWeeks, int monthOffset = 0)
    {
        DateTime firstDayOfMonth = GetDayOfMonth(someDayOfTheMonth.AddMonths(monthOffset), 1);
        DateTime firstDayOfTheWeek = GetDayOfWeek(firstDayOfMonth, DayOfWeek.Sunday);
        return GetDaysFrom(firstDayOfTheWeek, 7 * numberOfWeeks);
    }

    public static List<Day> GetDaysOfWeek(DateTime someDayOfTheWeek, int numberOfDays = 7)
    {
        return GetDaysFrom(GetDayOfWeek(someDayOfTheWeek, DayOfWeek.Sunday), numberOfDays);
    }

    #region Get Day Of Month
    public static Day GetDayOfMonth_Convert(DateTime someDayOfTheMonth, int requestedDay)
    {
        return new Day(GetDayOfMonth(someDayOfTheMonth, requestedDay));
    }
    public static DateTime GetDayOfMonth_Convert(Day someDayOfTheMonth, int requestedDay)
    {
        return GetDayOfMonth(someDayOfTheMonth.GetAnchorDateTime(), requestedDay);
    }
    public static DateTime GetDayOfMonth(DateTime someDayOfTheMonth, int requestedDay)
    {
        return someDayOfTheMonth.AddDays(requestedDay - someDayOfTheMonth.Day);
    }
    public static Day GetDayOfMonth(Day someDayOfTheMonth, int requestedDay)
    {
        return GetDayOfMonth_Convert(someDayOfTheMonth.GetAnchorDateTime(), requestedDay);
    }
    #endregion

    #region Get Day Of Week
    public static DateTime GetDayOfWeek_Convert(Day someDayOfTheWeek, DayOfWeek requestedDay)
    {
        return GetDayOfWeek(someDayOfTheWeek.GetAnchorDateTime(), requestedDay);
    }
    public static Day GetDayOfWeek_Convert(DateTime someDayOfTheWeek, DayOfWeek requestedDay)
    {
        return new Day(GetDayOfWeek(someDayOfTheWeek, requestedDay));
    }
    public static Day GetDayOfWeek(Day someDayOfTheWeek, DayOfWeek requestedDay)
    {
        return GetDayOfWeek_Convert(someDayOfTheWeek.GetAnchorDateTime(), requestedDay);
    }
    public static DateTime GetDayOfWeek(DateTime someDayOfTheWeek, DayOfWeek requestedDay)
    {
        return someDayOfTheWeek.AddDays((int)requestedDay - (int)someDayOfTheWeek.DayOfWeek);
    }
    #endregion

    #region GetStrings
    public static string GetMonthAbbreviation(int monthOfYear)
    {
        switch (monthOfYear)
        {
            case 1:
                return "Jan";
            case 2:
                return "F\u00E9v";
            case 3:
                return "Mar";
            case 4:
                return "Avr";
            case 5:
                return "Mai";
            case 6:
                return "Jun";
            case 7:
                return "Jul";
            case 8:
                return "Ao\u00FB";
            case 9:
                return "Sep";
            case 10:
                return "Oct";
            case 11:
                return "Nov";
            case 12:
                return "D\u00E9c";
            default:
                return "NaM";
        }
    }
    #endregion
}
