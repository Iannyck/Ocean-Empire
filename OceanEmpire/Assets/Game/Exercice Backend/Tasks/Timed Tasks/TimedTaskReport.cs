using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimedTaskReport
{
    /// <summary>
    /// Pas besoin de set cette variable manuellement.
    /// </summary>
    public DateTime reportCreatedOn;

    public enum State { PreemptivelyCancelled = 0, Skipped = 3, InterruptedInProgress = 1, Completed = 2 }

    //Mendatoire
    public State state;
    public string taskDescription;
    public ExerciseType exerciseType;
    public DateTime taskCreatedOn;
    public TimeSlot taskPlannedFor;
    public bool wasInstantTask;

    //Optionnel
    public ExerciseTrackingReport trackingReport;
    public HappyRating rating;

    public TimedTaskReport()
    {
        reportCreatedOn = DateTime.Now;
    }

    public static TimedTaskReport BuildFromCancelled(TimedTask timedTask)
    {
        TimedTaskReport report = new TimedTaskReport
        {
            state = State.PreemptivelyCancelled,
            taskDescription = timedTask.task.ToString(),
            trackingReport = null,
            rating = HappyRating.None,
            exerciseType = timedTask.task.GetExerciseType(),
            taskCreatedOn = timedTask.createdOn,
            taskPlannedFor = timedTask.timeSlot,
            wasInstantTask = timedTask is InstantTask
        };

        return report;
    }

    public static TimedTaskReport BuildFromInterrupted(TimedTask timedTask, ExerciseTrackingReport trackingReport)
    {
        TimedTaskReport report = new TimedTaskReport
        {
            state = State.InterruptedInProgress,
            taskDescription = timedTask.task.ToString(),
            trackingReport = trackingReport,
            rating = HappyRating.None,
            exerciseType = timedTask.task.GetExerciseType(),
            taskCreatedOn = timedTask.createdOn,
            taskPlannedFor = timedTask.timeSlot,
            wasInstantTask = timedTask is InstantTask,
        };

        return report;
    }

    public static TimedTaskReport BuildFromCompleted(TimedTask timedTask, ExerciseTrackingReport trackingReport, HappyRating happyRating)
    {
        TimedTaskReport report = new TimedTaskReport
        {
            state = State.Completed,
            trackingReport = trackingReport,
            taskDescription = timedTask.task.ToString(),
            rating = happyRating,
            exerciseType = timedTask.task.GetExerciseType(),
            taskCreatedOn = timedTask.createdOn,
            taskPlannedFor = timedTask.timeSlot,
            wasInstantTask = timedTask is InstantTask,
        };

        return report;
    }

    public TimedTaskReport(ExerciseTrackingReport exerciseReport, DateTime taskCreatedOn, TimeSlot plannedTimeSlot, bool isInstantTask, HappyRating rating = HappyRating.None)
    {
        reportCreatedOn = DateTime.Now;


        this.trackingReport = exerciseReport;
        this.rating = rating;
        taskPlannedFor = plannedTimeSlot;
        this.taskCreatedOn = taskCreatedOn;
    }

    public override string ToString()
    {
        return "State: " + state.ToString() +
            "\nExercise type: " + exerciseType.ToString()
            +"\nTask: " + taskDescription
            +"\nReport created on: " + reportCreatedOn.ToString() +
            "\nTask created on: " + taskCreatedOn.ToString() +
            "\n---Task planned for---\n" + taskPlannedFor.ToString() + "\n---"
            + "\nWas instant task: " + wasInstantTask.ToString() +
            "\nHappy rating: " + rating.ToString() +
            "\n---Tracking report---\n" + trackingReport.ToString() + "\n---";
    }
}
