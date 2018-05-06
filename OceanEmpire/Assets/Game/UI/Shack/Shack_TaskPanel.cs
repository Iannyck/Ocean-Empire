using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shack_TaskPanel : MonoBehaviour
{
    public CanvasGroup[] canvasToHide;
    public SceneInfo whenToPlanScene;

    public PanelState State { get; private set; }
    public enum PanelState
    {
        ReadyToCreate,
        Creation_SelectingTask,
        Creation_InCalendar
    }

    private Tween currentAnim;
    private Task latestTask;

    public void LaunchTaskCreation()
    {
        if (State != PanelState.ReadyToCreate)
            return;


        State = PanelState.Creation_SelectingTask;

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
                        ShowShackUI();
                        State = PanelState.ReadyToCreate;
                        listController.CloseWindow();
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
                                if(ScheduleTaskAt(task, new TimeSlot(DateTime.Now, task.GetMaxDurationAsTimeSpan())))
                                {
                                    State = PanelState.ReadyToCreate;
                                    listController.CloseWindow();
                                }
                            };
                            whenToPlanController.OnLaterClick = () =>
                            {
                                // Open calendar en schedule later
                                SelectTimeSlotForTask(latestTask,
                                    (selectedTimeSlot) =>
                                    {
                                        // On result selected
                                        if(ScheduleTaskAt(latestTask, selectedTimeSlot))
                                        {
                                            State = PanelState.ReadyToCreate;
                                        }
                                    },
                                    () =>
                                    {
                                        // On cancel
                                        ShowShackUI();
                                        State = PanelState.ReadyToCreate;
                                    });
                                listController.CloseWindow();
                            };
                        });
                    }
                });
        });
    }

    private bool ScheduleTaskAt(Task task, TimeSlot selectedTimeSlot)
    {
        // On result selected
        ScheduledBonus schedule = new ScheduledBonus(selectedTimeSlot, ScheduledBonus.DefaultBonus())
        {
            task = task,
            displayPadding = true,
            minutesOfPadding = 15
        };
        if (Calendar.instance.AddSchedule(schedule))
        {
            // Yay ! C'est fini
            ShowShackUI();
            State = PanelState.ReadyToCreate;
            return true;
        }
        else
        {
            MessagePopup.DisplayMessage("Plage horaire déjà occupé.");
            State = PanelState.ReadyToCreate;
            return false;
        }
    }


    private void SelectTimeSlotForTask(Task task, Action<TimeSlot> resultCallback, Action cancelCallback)
    {
        if (State == PanelState.Creation_InCalendar)
            return;
        State = PanelState.Creation_InCalendar;

        bool hasSentResult = false;

        // Request settings
        CalendarRequest.Settings settings = new CalendarRequest.Settings()
        {
            windowHeaderTitle = "Quel jour?",
            scheduleButtonText = "Plannifier l'exercice"
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

            //TimeSpan FIFTEEN_MINUTES = new TimeSpan(0, 15, 0);

            //DateTime now = DateTime.Now;
            DateTime scheduleStart = date;
            //if (scheduleStart < now)
            //    scheduleStart = now;

            DateTime scheduleEnd = scheduleStart + task.GetMaxDurationAsTimeSpan();

            TimeSlot timeSlot = new TimeSlot(scheduleStart, scheduleEnd);

            if (Calendar.instance.IsOverlappingWithSchedule(timeSlot))
            {
                MessagePopup.DisplayMessage("La plage horaire est déjà occupé.");
            }
            else
            {
                resultCallback(timeSlot);
                hasSentResult = true;
                handle.CloseWindow();
            }
        };
    }



    #region FadeIn / FadeOut des canvas du Shack
    private void HideShackUIInstant()
    {
        KillAnim();
        SetAlpha(0);
    }
    private void HideShackUI(TweenCallback onComplete = null)
    {
        KillAnim();

        float alpha = 1;
        currentAnim = DOTween.To(() => alpha, (x) =>
        {
            alpha = x;
            SetAlpha(alpha);
        }, 0, 0.35f).OnComplete(onComplete);
    }

    private void ShowShackUIInstant()
    {
        KillAnim();
        SetAlpha(1);
    }
    private void ShowShackUI(TweenCallback onComplete = null)
    {
        KillAnim();

        float alpha = 0;
        currentAnim = DOTween.To(() => alpha, (x) =>
        {
            alpha = x;
            SetAlpha(alpha);
        }, 1, 0.35f).OnComplete(onComplete);
    }

    private void SetAlpha(float alpha)
    {
        for (int i = 0; i < canvasToHide.Length; i++)
        {
            canvasToHide[i].alpha = alpha;
        }
    }

    void KillAnim()
    {
        if (currentAnim != null && currentAnim.IsActive())
        {
            currentAnim.Kill();
        }
    }

    void OnDestroy()
    {
        KillAnim();
    }
    #endregion
}
