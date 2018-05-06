using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shack : MonoBehaviour
{
    public const string SCENENAME = "Shack";

    public CanvasGroup hud;
    public CanvasGroup mainCanvas;
    public CanvasGroup alwayVisibleHud;

    [Header("Hub")]
    public QuestPanel questPanel;

    [Header("Environement")]
    public Shack_Environment shack_Environment;

    [Header("Recolte")]
    public FishingFrenzyWidget fishingFrenzyWidget;
    public Shack_CallToAction recolteCallToAction;

    [Header("Calendar")]
    public SceneInfo calendarScene;

    void Start()
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            CheckFishingFrenzy();
            if (fishingFrenzyWidget != null)
                fishingFrenzyWidget.OnStateUpdated.AddListener(CheckFishingFrenzy);

            ApplyMapChange(MapManager.Instance.MapData);
            MapManager.Instance.OnMapSet += ApplyMapChange;
        });
    }

    private void ApplyMapChange(int mapIndex, MapData obj) { ApplyMapChange(obj); }
    private void ApplyMapChange(MapData mapData)
    {
        shack_Environment.ApplyMapData(mapData);
    }

    void OnDestroy()
    {
        if (fishingFrenzyWidget != null)
            fishingFrenzyWidget.OnStateUpdated.RemoveListener(CheckFishingFrenzy);

        if (MapManager.Instance != null)
            MapManager.Instance.OnMapSet -= ApplyMapChange;
    }

    void CheckFishingFrenzy()
    {
        recolteCallToAction.enabled = FishingFrenzy.Instance.shopCategory.IsAvailable &&
            FishingFrenzy.Instance.State == FishingFrenzy.EffectState.Available;
    }

    public void OnReturnFromShop()
    {
        fishingFrenzyWidget.UpdateVisibility();
        CheckFishingFrenzy();
        questPanel.UpdateContent();
    }

    public void LaunchGame()
    {
        GameSettings gameSettings = new GameSettings(MapManager.Instance.MapData, true);

        if (FishingFrenzy.Instance != null
            && FishingFrenzy.Instance.shopCategory.IsAvailable
            && FishingFrenzy.Instance.State == FishingFrenzy.EffectState.Available)
        {
            FishingFrenzy.Instance.Activate();
        }
        LoadingScreen.TransitionTo(GameBuilder.SCENENAME, new ToRecolteMessage(gameSettings), true);
    }

    public bool IsInCalendar { get; private set; }

    public void OpenCalendar()
    {
        if (IsInCalendar)
            return;
        IsInCalendar = true;

        // Request settings
        CalendarRequest.Settings settings = new CalendarRequest.Settings()
        {
            windowHeaderTitle = "Quel jour?",
            scheduleButtonText = "Plannifier l'exercice"
        };

        // Launch request
        var handle = CalendarRequest.LaunchRequest(settings);
        handle.onWindowExitStarted = () =>
        {
            mainCanvas.alpha = 1;
            hud.alpha = 1;
            alwayVisibleHud.alpha = 1;
        };
        handle.onWindowIntroComplete = () =>
        {
            mainCanvas.alpha = 0;
            hud.alpha = 0;
            alwayVisibleHud.alpha = 0;
        };
        handle.onWindowExitComplete = () => IsInCalendar = false;

        // When the player picks a time in the calendar
        handle.onTimePicked = (date) =>
        {
            Debug.Log("DateTime picked: " + date);
            Task task = new Task(5, 10, 25);

            //TimeSpan FIFTEEN_MINUTES = new TimeSpan(0, 15, 0);

            //DateTime now = DateTime.Now;
            DateTime scheduleStart = date;
            //if (scheduleStart < now)
            //    scheduleStart = now;

            DateTime scheduleEnd = scheduleStart + task.GetMaxDurationAsTimeSpan();
            
            TimeSlot timeSlot = new TimeSlot(scheduleStart, scheduleEnd);

            ScheduledBonus schedule = new ScheduledBonus(timeSlot, ScheduledBonus.DefaultBonus())
            {
                task = task,
                displayPadding = true,
                minutesOfPadding = 15
            };

            if (!Calendar.instance.AddSchedule(schedule))
            {
                MessagePopup.DisplayMessage("La plage horaire est déjà occupé.");
            }
            else
            {
                handle.CloseWindow();
            }
        };
    }
}