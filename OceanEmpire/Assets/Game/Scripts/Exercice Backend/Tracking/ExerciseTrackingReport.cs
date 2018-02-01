using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExerciseTrackingReport
{
    public enum State { Abandonned = 0, Completed = 1, UserSaidItWasCompleted = 2, Incomplete = 3 }

    /// <summary>
    /// Pourcentage d'activité (50% = à été actif pendant la moitié du temps)
    /// </summary>
    public float activityRate;

    /// <summary>
    /// L'état de la tache
    /// </summary>
    public State state;

    /// <summary>
    /// Le début de la tache, et la durée total que ça a pris pour completer l'exercice
    /// </summary>
    public TimeSlot timeSlot;

    /// <summary>
    /// Liste des probabilités
    /// </summary>
    public List<float> probabilities;

    private ExerciseTrackingReport() { }

    public static ExerciseTrackingReport BuildFromNonInterrupted(ActivityAnalyser.Report activityReport)
    {
        DateTime taskStartTime = activityReport.task.timeSlot.start;
        return new ExerciseTrackingReport()
        {
            activityRate = activityReport.activityRate,
            state = activityReport.complete ? State.Completed : State.Incomplete,
            timeSlot = new TimeSlot(taskStartTime, activityReport.exerciceEnd - taskStartTime),
            probabilities = activityReport.probabilities
        };
    }
    public static ExerciseTrackingReport BuildFromAbandonned(ActivityAnalyser.Report activityReport)
    {
        DateTime taskStartTime = activityReport.task.timeSlot.start;
        return new ExerciseTrackingReport()
        {
            activityRate = activityReport.activityRate,
            state =  State.Abandonned,
            timeSlot = new TimeSlot(taskStartTime, activityReport.exerciceEnd - taskStartTime),
            probabilities = activityReport.probabilities
        };
    }
    public static ExerciseTrackingReport BuildFrom_UserSaidItWasCompleted(ActivityAnalyser.Report activityReport)
    {
        DateTime taskStartTime = activityReport.task.timeSlot.start;
        return new ExerciseTrackingReport()
        {
            activityRate = activityReport.activityRate,
            state = State.UserSaidItWasCompleted,
            timeSlot = new TimeSlot(taskStartTime, activityReport.exerciceEnd - taskStartTime),
            probabilities = activityReport.probabilities
        };
    }

    public override string ToString()
    {
        return "Activity rate: " + activityRate.ToString()
            + "\nState: " + state.ToString()
            + "\nTimeslot:\n" + timeSlot.ToString();
    }
}
