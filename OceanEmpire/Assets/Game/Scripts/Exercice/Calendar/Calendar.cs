﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

using UnityEngine;
using CCC.Persistence;

public class Calendar : MonoPersistent
{
    private const string SAVEKEY_FUTURE_BONIFIEDTIMES = "futureBT";
    private const string SAVEKEY_PAST_BONIFIEDTIMES = "pasBT";

    [SerializeField, Suffix("seconds")]
    private float checkConcludeEvery = 4;

    /// <summary>
    /// Ordonné du plus vieux au plus récent
    /// </summary>
    private List<BonifiedTime> presentAndFutureBonifiedTimes = new List<BonifiedTime>();
    /// <summary>
    /// Ordonné du plus vieux au plus récent
    /// </summary>
    private List<BonifiedTime> pastBonifiedTimes = new List<BonifiedTime>();
    [SerializeField] private DataSaver dataSaver;

    public bool log = true;
    public event SimpleEvent OnBonifiedTimeAdded;
    public static Calendar instance;

    /// <summary>
    /// Ordonné du plus vieux au plus récent
    /// </summary>
    public ReadOnlyCollection<BonifiedTime> GetPresentAndFutureBonifiedTimes() { return presentAndFutureBonifiedTimes.AsReadOnly(); }
    /// <summary>
    /// Ordonné du plus vieux au plus récent
    /// </summary>
    public ReadOnlyCollection<BonifiedTime> GetPastBonifiedTimes() { return pastBonifiedTimes.AsReadOnly(); }

    protected void Awake()
    {
        dataSaver.OnReassignData += FetchDataFromSaver;
    }

    public override void Init(Action onComplete)
    {
        instance = this;
        FetchDataFromSaver();
        onComplete();

        PersistentLoader.LoadIfNotLoaded(() => StartCoroutine(PeriodicCheck()));
    }

    IEnumerator PeriodicCheck()
    {
        while (true)
        {
            CheckBonifiedTimes();
            yield return new WaitForSecondsRealtime(checkConcludeEvery);
        }
    }

    private void CheckBonifiedTimes()
    {
        DateTime now = DateTime.Now;
        for (int i = 0; i < presentAndFutureBonifiedTimes.Count; i++)
        {
            int timeslotRelation = presentAndFutureBonifiedTimes[i].timeslot.IsOverlappingWith(now);

            //The bonifiedTime is in the future
            if (timeslotRelation == 1)
                return;

            //The bonifiedTime is in the past
            if (timeslotRelation == -1)
                ConcludeBonifiedTime(i);
        }
    }

    private void ConcludeBonifiedTime(int index, bool andSave = true)
    {
        pastBonifiedTimes.Add(presentAndFutureBonifiedTimes[index]);
        presentAndFutureBonifiedTimes.RemoveAt(index);
        if (log)
            Debug.Log("Bonified Time concluded: " + index);
        if (andSave)
            ApplyDataToSaver(andSave);
    }

    public bool AddBonifiedTime(BonifiedTime bonifiedTime)
    {
        if (!IsTimeSlotBonified(bonifiedTime.timeslot))
        {
            presentAndFutureBonifiedTimes.SortedAdd(bonifiedTime, (a, b) => a.timeslot.start.CompareTo(b.timeslot.start));

            ApplyDataToSaver(true);

            if (log)
                Debug.Log("BonifiedTime ajouté au calendrier avec succès.");

            if (OnBonifiedTimeAdded != null)
                OnBonifiedTimeAdded();

            return true;
        }
        else
        {
            if (log)
                Debug.LogWarning("Le BonifiedTime n'a pas pu être ajouté au calendrier. " +
                    "La timeslot est déjà utilisé.");
            return false;
        }
    }

    /// <summary>
    /// ( ͡° ͜ʖ ͡°)
    /// </summary>
    public bool IsTimeSlotBonified(TimeSlot timeslot)
    {
        for (int i = 0; i < presentAndFutureBonifiedTimes.Count; i++)
        {
            int result = timeslot.IsOverlappingWith(presentAndFutureBonifiedTimes[i].timeslot);
            if (result == 0)
                return true;
            else if (result == -1)
                return false;
        }
        return false;
    }

    private void ApplyDataToSaver(bool andSave)
    {
        dataSaver.SetObjectClone(SAVEKEY_FUTURE_BONIFIEDTIMES, presentAndFutureBonifiedTimes);
        dataSaver.SetObjectClone(SAVEKEY_PAST_BONIFIEDTIMES, pastBonifiedTimes);

        if (andSave)
        {
            dataSaver.SaveAsync();
        }
    }

    private void FetchDataFromSaver()
    {
        presentAndFutureBonifiedTimes = dataSaver.GetObjectClone(SAVEKEY_FUTURE_BONIFIEDTIMES) as List<BonifiedTime>;
        if (presentAndFutureBonifiedTimes == null)
            presentAndFutureBonifiedTimes = new List<BonifiedTime>();

        pastBonifiedTimes = dataSaver.GetObjectClone(SAVEKEY_PAST_BONIFIEDTIMES) as List<BonifiedTime>;
        if (pastBonifiedTimes == null)
            pastBonifiedTimes = new List<BonifiedTime>();
    }

    public List<BonifiedTime> GetAllBonifiedTimesOn(Day day)
    {
        List<BonifiedTime> result = new List<BonifiedTime>();

        for (int i = pastBonifiedTimes.Count - 1; i >= 0; i--)
        {
            DateTime entry = pastBonifiedTimes[i].timeslot.start;

            int entryIsInThePast = day.IsInTheSameDay(entry);

            // Stop !
            if (entryIsInThePast == 1)
            {
                break;
            }

            // Same day
            if (entryIsInThePast == 0)
                result.Add(pastBonifiedTimes[i]);
        }

        for (int i = 0; i < presentAndFutureBonifiedTimes.Count; i++)
        {
            DateTime entry = presentAndFutureBonifiedTimes[i].timeslot.start;

            int entryIsInThePast = day.IsInTheSameDay(entry);

            // Entry is in the future, Stop !
            if (entryIsInThePast == -1)
            {
                break;
            }

            // Same day
            if (entryIsInThePast == 0)
                result.Add(presentAndFutureBonifiedTimes[i]);
        }

        return result;
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

        /// <summary>
        /// 1 = time est dans le passé  0 = time est dans la même journée  -1 = time est dans le futur
        /// </summary>
        public int IsInTheSameDay(DateTime time)
        {
            if (time.Year < year)
                return 1;
            if (time.Year > year)
                return -1;
            if (time.Month < monthOfYear)
                return 1;
            if (time.Month > monthOfYear)
                return -1;
            if (time.Day < dayOfMonth)
                return 1;
            if (time.Day > dayOfMonth)
                return -1;
            return 0;
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
    public static string GetMonthName(int monthOfYear)
    {
        switch (monthOfYear)
        {
            case 1:
                return "Janvier";
            case 2:
                return "Février";
            case 3:
                return "Mars";
            case 4:
                return "Avril";
            case 5:
                return "Mai";
            case 6:
                return "Juin";
            case 7:
                return "Juillet";
            case 8:
                return "Août";
            case 9:
                return "Septembre";
            case 10:
                return "Octobre";
            case 11:
                return "Novembre";
            case 12:
                return "Décembre";
            default:
                return "NaM";
        }
    }
    public static string GetDayOfTheWeekName(DayOfWeek dayOfTheWeek)
    {
        switch (dayOfTheWeek)
        {
            case DayOfWeek.Sunday:
                return "Dimanche";
            case DayOfWeek.Monday:
                return "Lundi";
            case DayOfWeek.Tuesday:
                return "Mardi";
            case DayOfWeek.Wednesday:
                return "Mercredi";
            case DayOfWeek.Thursday:
                return "Jeudi";
            case DayOfWeek.Friday:
                return "Vendredi";
            case DayOfWeek.Saturday:
                return "Samedi";
            default:
                return "Error";
        }
    }
    #endregion
}