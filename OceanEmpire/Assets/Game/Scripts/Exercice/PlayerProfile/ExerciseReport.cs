using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseReport
{
    public TimeSlot timeSlot;
    public float volume;
    // exercice
    // objectif
    public Etat etat;

    public enum Etat
    {
        encours = 0,
        fini = 1
    }

    public ExerciseReport(TimeSlot timeSlot, float volume)
    {
        this.timeSlot = timeSlot;
        this.volume = volume;
        etat = Etat.encours;
    }

    public ExerciseReport()
    {
        timeSlot = new TimeSlot();
        volume = 0;
        etat = Etat.encours;
    }

    public string GetString()
    {
        return timeSlot.ToString() + volume + etat;
    }
}
