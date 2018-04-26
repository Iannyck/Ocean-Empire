using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseReport
{
    public TimeSlot timeSlot;
    public float volume;
    PossibleExercice.PlannedExercice exercice;
    public Etat etat;

    public enum Etat
    {
        reussi = 0,
        echec = 1
    }

    public ExerciseReport(TimeSlot timeSlot, PossibleExercice.PlannedExercice exercice, float volume)
    {
        this.timeSlot = timeSlot;
        this.volume = volume;
        this.exercice = exercice;
        etat = Etat.echec;
    }

    public ExerciseReport()
    {
        timeSlot = new TimeSlot();
        volume = 0;
        etat = Etat.echec;
        exercice = null;
    }

    public string GetString()
    {
        return "|" + timeSlot.ToString() + "-" + exercice.type + "-" + exercice.difficulty + "-" + exercice.minAmount + "-" + volume + "-" + etat;
    }
}
