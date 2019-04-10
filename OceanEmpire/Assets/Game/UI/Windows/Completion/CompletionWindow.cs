using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class CompletionWindow : WindowAnimation
{
    public InstantExerciseChoice_Item taskUI;
    public Text dateText;
    public Text timeSlotText;
    public Text rewardText;

    [SerializeField]
    public Scene InstantExercise;
    public SceneInfo whenToPlanScene;
    
    public bool jobDone = false;
    [SerializeField]
    public int maxUtilisation = 3;

    public PanelState State { get; private set; }
    public enum PanelState
    {
        ReadyToCreate,
        Creation_SelectingTask,
        Creation_InCalendar
    }

    private Task latestTask;

    Action onComplete;

    protected void Update(){
        if(jobDone){
            return;
        }
        GameObject buttonRestart = GameObject.Find("Restart");
        if(latestTask!=null && !jobDone){
            if(latestTask.nbUtilisation >= maxUtilisation){  
            buttonRestart.SetActive(false);
            jobDone = true;
            }
        }
        

    }

    public void FillContent(PlannedExerciceRewarder.Report report, Action onComplete)
    {
        this.onComplete = onComplete;

        latestTask = report.schedule.task;
        jobDone = false;

        var start = report.schedule.timeSlot.start;
        var end = report.schedule.timeSlot.end;
        
        taskUI.FillContent(report.schedule.task);
        dateText.text = "" +
            Calendar.GetDayOfTheWeekName(start.DayOfWeek) + ", " +
            start.Day + " " +
            Calendar.GetMonthAbbreviation(start.Month);
        timeSlotText.text = "Entre <color=#f>" + start.ToCondensedTimeOfDayString() + "</color>\net <color=#f>"
            + end.ToCondensedTimeOfDayString() + "</color>";

        rewardText.text = "+ " + report.schedule.task.ticketReward;
    }

    public void ContinueClick()
    {
        if (onComplete != null)
            onComplete();

        Close();
    }

    public void RestartActivity(){
        
        if (State != PanelState.ReadyToCreate)
            return;

        State = PanelState.Creation_SelectingTask;

        /* Action completeAnim = null;
        Action completionAction = () =>
        {
            //completeAnim();
            if (onComplete != null)
                onComplete();
            
        };
*/
        //Récupérer le bouton restart
        GameObject buttonRestart = GameObject.Find("Restart");
        buttonRestart.SetActive(false);
        
        HideCompletionUI();
        InstantExerciseChoice.ProposeTasks(
            (listController, task) =>
            {
                // PLAYER HAS CHOSEN A TASK   (or cancelled)
                //latestTask = task;
                if (task == null)
                {
                    // Player has cancelled
                    listController.CloseWindow();
                    BackToNormal();
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
                                task.nbUtilisation += latestTask.nbUtilisation;
                                listController.CloseWindow();
                                BackToNormal();
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
                                    BackToNormal();
                                },
                                () =>
                                {
                                    // On cancel
                                    buttonRestart.SetActive(true);
                                    BackToNormal();
                                });
                            listController.CloseWindow();
                        };
                    });
                }
            }
        );
        
    }
    
    private void BackToNormal()
    {
        State = PanelState.ReadyToCreate;
        ShowCompletionUI();
        //if (onComplete != null)
        //    onComplete();
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
        if (DateTime.Now < bookedStart)
        {
            Debug.Log("notif test");
            NotificationManager.SendWithAppIcon((bookedStart-DateTime.Now),
                        "Rappel",
                        "Marche de " + task.minDuration + " à " + task.maxDuration + " min",
                        new Color(0, 0.6f, 1),
                        NotificationIcon.Message);
        }
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


    private void HideCompletionUI(TweenCallback onComplete = null)
    {
        //shack_Canvas.HideAll(Shack_Canvas.Filter.None, onComplete);
        gameObject.SetActive(false);
    }

    private void ShowCompletionUI(TweenCallback onComplete = null)
    {
        //shack_Canvas.ShowAll(Shack_Canvas.Filter.None, onComplete);
        gameObject.SetActive(true);
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

}
