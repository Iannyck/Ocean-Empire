using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PrintTime
{
    const string fJour = " jour ";
    const string fJours = " jours ";
    const string fHours = " heures ";
    const string fHour = " heure ";
    const string fMinutes = " minutes ";
    const string fMinute = " minute ";
    const string fSeconde = " seconde ";
    const string fSecondes = " secondes ";


    const string sJour = " j";
    const string sHour = " h";
    const string sMinute = " m";
    const string sSeconde = " s";
   

    public static string ShortString(TimeSpan time )
    {
        string value = "";

        int days = time.Days;
        if (days > 1)
        {
            value += days.ToString();
            value += sJour ;
        }

        int hours = time.Hours;
        if (hours > 1)
        {
            if (value != "")
                value += ", ";
            value += hours.ToString();
            value +=  sHour;
        }

        int minutes = time.Minutes;
        if (minutes > 1)
        {
            if (value != "")
                value += ", ";
            value += minutes.ToString();
            value += sMinute;
        }

        int secondes = time.Seconds;
        if (secondes > 1)
        {
            if (value != "")
                value += ", ";
            value += secondes.ToString();
            value += sSeconde;
        }

        if (value == "")
            value = "aucun temps";

        return value;
    }


    public static string FullString(TimeSpan time)
    {
        string value = "";

        int days = time.Days;
        if (days > 1)
        {
            value += days.ToString();
            value += (days == 1 ? fJour : fJours);
        }

        int hours = time.Hours;
        if (hours > 1)
        {
            value += hours.ToString();
            value += (hours == 1 ? fHour : fHours);
        }

        int minutes = time.Minutes;
        if (minutes > 1)
        {
            value += minutes.ToString();
            value += (minutes == 1 ? fMinute : fMinutes);
        }

        int secondes = time.Seconds;
        if (secondes > 1)
        {
            value += secondes.ToString();
            value += (secondes == 1 ? fSeconde : fSecondes);
        }

        if (value == "")
            value = "aucun temps";

        return value;
    }
}
