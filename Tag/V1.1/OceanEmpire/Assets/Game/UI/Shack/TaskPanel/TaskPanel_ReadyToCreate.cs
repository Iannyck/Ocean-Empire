using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPanel_ReadyToCreate : MonoBehaviour, ITaskPanelState
{
    public Shack_Canvas shack_Canvas;
    public SceneInfo whenToPlanScene;
    public ScriptableActionQueue shackAnimQueue;

    public PanelState State { get; private set; }
    public enum PanelState
    {
        ReadyToCreate,
        Creation_SelectingTask,
        Creation_InCalendar
    }

    private Tween currentAnim;
    private Task latestTask;


    public void Enter(Action onComplete)
    {
        gameObject.SetActive(true);
        if (onComplete != null)
            onComplete();
    }

    public void Exit(Action onComplete)
    {
        gameObject.SetActive(false);
        if (onComplete != null)
            onComplete();
    }

    public void FillContent(object data)
    {
    }

    public void LaunchTaskCreation() { LaunchTaskCreation(null); }
    public void LaunchTaskCreation(Action onComplete)
    {
        if (State != PanelState.ReadyToCreate)
            return;

        State = PanelState.Creation_SelectingTask;
        Action completeAnim = null;
        Action completionAction = () =>
        {
            completeAnim();
            if (onComplete != null)
                onComplete();
        };

        shackAnimQueue.ActionQueue.AddAction(() =>
        {
            // Fade out all canvas
            HideShackUI(() =>
            {
                InstantExerciseChoice.ProposeTasks(
                    (listController, task) =>
                    {
                        // PLAYER HAS CHOSEN A TASK   (or cancelled)
                        latestTask = task;

                        if (task == null)
                        {
                            // Player has cancelled
                            listController.CloseWindow();
                            BackToNormal(completionAction);
                        }
                        else
                        {
                            // Task chosen! When to plan?
                            Scenes.LoadAsync(whenToPlanScene, (s) =>
                                {
                                    var whenToPlanController = s.FindRootObject<WhenToPlanWindow>();
                                    whenToPlanController.OnNowClick = () =>
                                    {
                                        // Schedule now!
                                        var now = DateTime.Now;
                                        if (ScheduleTaskAt(task, now, ref now))
                                        {
                                            listController.CloseWindow();
                                            BackToNormal(completionAction);
                                        }
                                    };
                                    whenToPlanController.OnLaterClick = () =>
                                        {
                                            // Open calendar en schedule later
                                            SelectDateTimeForTask(latestTask,
                                                (date) =>
                                                {
                                                    // On result selected
                                                    ScheduleTaskAt(latestTask, date);
                                                    BackToNormal(completionAction);
                                                },
                                                () =>
                                                {
                                                    // On cancel
                                                    BackToNormal(completionAction);
                                                });
                                            listController.CloseWindow();
                                        };
                                });
                        }
                    });
            });
        }, 0, out completeAnim);
    }

    private void BackToNormal(Action onComplete)
    {
        State = PanelState.ReadyToCreate;
        ShowShackUI();
        if (onComplete != null)
            onComplete();
    }

    private bool ScheduleTaskAt(Task task, DateTime startTime)
    {
        var now = DateTime.Now;
        return ScheduleTaskAt(task, startTime, ref now);
    }
    private bool ScheduleTaskAt(Task task, DateTime startTime, ref DateTime now)
    {
        // 1 - Nous voulons, si possible, paddé de 15 min à l'avance au cas où les bond de 15 min du calendrier
        //      ne satisfont pas le plan du joueur.
        //      Ex: Il veut débuter sa marche à 4h25, alors il la planifie à 4h30
        // 2 - Nous voulons assurer que le joueur a AU MOINS 2x le temps minimal pour accomplir sa tache
        //      au cas où il prendrait des pauses régulièrements
        //      Ex: s'il a faire une marche de 20 à 30 min, il doit avoir AU MOINS 40 min pour la faire
        // 3 - Nous voulons assurer un MINIMUM de 5 min pour compenser avec le temps de préparation.
        //      (ex: mettre ces chaussure, son manteau, sortir de l'immeuble, etc.)
        //      Ex: le joueur prend 6 min pour terminer ses affaires, changer d'habit, sortir dehors etc.
        //          Si sa tâche était de faire entre 7 et 10 min de marche et qu'elle était booked sur 15 min,
        //          il n'aura peut-être pas le temps de finir l'exercice

        var FIFTEEN_MINUTES = new TimeSpan(0, 15, 0);
        var FIVE_MINUTES = new TimeSpan(0, 5, 0);

        // 1
        var bookedStart = startTime - FIFTEEN_MINUTES;

        // 3
        var remainingPadding = TimeSpan.Zero;
        if (bookedStart < now)
        {
            remainingPadding = now - bookedStart;
            if (remainingPadding > FIVE_MINUTES)
                remainingPadding = FIVE_MINUTES;
            bookedStart = now;
        }

        // 2
        var minBookedDuration = task.GetMinDurationAsTimeSpan();
        minBookedDuration += minBookedDuration; // 2x

        // 2 & 3
        var bookedEnd = (startTime + minBookedDuration + remainingPadding).Ceiled(FIFTEEN_MINUTES);

        TimeSlot bookedTimeslot = new TimeSlot(bookedStart, bookedEnd);




        // On result selected
        Schedule schedule = new Schedule(bookedTimeslot)
        {
            task = task,
            requiresConculsion = true
        };

        if (Calendar.instance.AddSchedule(schedule))
        {
            Logger.Log(Logger.Category.PlannedExercise, "Planned: " + schedule.ToString());

            // Yay ! C'est fini
            PlannedExerciceRewarder.Instance.ForceAnalyseCheck();
            return true;
        }
        else
        {
            MessagePopup.DisplayMessage("Plage horaire déjà occupé.");
            return false;
        }
    }

    private void SelectDateTimeForTask(Task task, Action<DateTime> resultCallback, Action cancelCallback)
    {
        if (State == PanelState.Creation_InCalendar)
            return;
        State = PanelState.Creation_InCalendar;

        bool hasSentResult = false;

        // Request settings
        CalendarRequest.Settings settings = new CalendarRequest.Settings()
        {
            windowHeaderTitle = "Quel jour?",
            scheduleButtonText = "Planifier l'exercice"
        };

        // Launch request
        var handle = CalendarRequest.LaunchRequest(settings);
        //handle.onWindowIntroComplete = HideShackUIInstant;
        //handle.onWindowExitStarted = ShowShackUI;
        handle.onWindowExitComplete = () =>
        {
            if (!hasSentResult)
                cancelCallback();
        };

        // When the player picks a time in the calendar
        handle.onTimePicked = (date) =>
        {
            Debug.Log("DateTime picked: " + date);
            resultCallback(date);
            hasSentResult = true;
            handle.CloseWindow();
        };
    }


    #region FadeIn / FadeOut des canvas du Shack
    private void HideShackUIInstant()
    {
        shack_Canvas.HideAllInstant(Shack_Canvas.Filter.None);
    }
    private void HideShackUI(TweenCallback onComplete = null)
    {
        shack_Canvas.HideAll(Shack_Canvas.Filter.None, onComplete);
    }

    private void ShowShackUIInstant()
    {
        shack_Canvas.ShowAllInstant(Shack_Canvas.Filter.None);
    }
    private void ShowShackUI(TweenCallback onComplete = null)
    {
        shack_Canvas.ShowAll(Shack_Canvas.Filter.None, onComplete);
    }
    #endregion
}
