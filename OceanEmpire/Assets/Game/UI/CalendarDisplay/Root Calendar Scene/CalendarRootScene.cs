using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CalendarRootScene : MonoBehaviour
{
    public const string SCENENAME = "CalendarRootScene";

    [ReadOnly]
    public CalendarGrid_Controller gridCalendar;
    [ReadOnly]
    public CalendarScroll_Controller scrollCalendar;
    [ReadOnly]
    public DayInspector dayInspector;

    [SerializeField] public Camera calendarCamera;

    private bool entranceComplete = false;
    private Action onEntranceComplete;

    public enum CalendarType { Scroll = 0, Grid = 1 }
    public CalendarType defaultType = CalendarType.Scroll;

    private void Start()
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            InitQueue queue = new InitQueue(AllScenesLoaded);

            FetchCalendarType(CalendarType.Scroll, queue.Register());
            FetchCalendarType(CalendarType.Grid, queue.Register());
            FetchDayInspector(queue.Register());

            queue.MarkEnd();
        });
    }

    public static void OpenCalendar(Action onLoadComplete, Action onEntranceComplete)
    {
        Scenes.Load(SCENENAME, LoadSceneMode.Additive, (scene) =>
        {
            if (onLoadComplete != null)
                onLoadComplete();

            var calendarRoot = scene.FindRootObject<CalendarRootScene>();
            if (calendarRoot.entranceComplete)
            {
                if (onEntranceComplete != null)
                    onEntranceComplete();
            }
            else
                calendarRoot.onEntranceComplete = onEntranceComplete;
        });
    }

    private void AllScenesLoaded()
    {
        TweenCallback onComplete = () =>
        {
            entranceComplete = true;
            if (onEntranceComplete != null)
                onEntranceComplete();
        };

        switch (defaultType)
        {
            case CalendarType.Scroll:
                if (!scrollCalendar.IsShown)
                    scrollCalendar.Show(onComplete);
                break;
            case CalendarType.Grid:
                if (!gridCalendar.IsShown)
                    gridCalendar.Show(onComplete);
                break;
        }

        gridCalendar.root = this;
        scrollCalendar.root = this;
        dayInspector.root = this;
    }

    public void UnloadAll()
    {
        if (gridCalendar != null)
            Scenes.UnloadAsync(gridCalendar.gameObject.scene);
        if (scrollCalendar != null)
            Scenes.UnloadAsync(scrollCalendar.gameObject.scene);
        if (dayInspector != null)
            Scenes.UnloadAsync(dayInspector.gameObject.scene);

        Scenes.UnloadAsync(gameObject.scene);
    }

    private void FetchDayInspector(Action onComplete)
    {
        Action<Scene> fetcher = (Scene scene) =>
        {
            dayInspector = scene.FindRootObject<DayInspector>();
            if (onComplete != null)
                onComplete();
        };

        if (!Scenes.IsActiveOrBeingLoaded(DayInspector.SCENENAME))
            Scenes.Load(DayInspector.SCENENAME, LoadSceneMode.Additive, fetcher);
        else
            fetcher(Scenes.GetActive(DayInspector.SCENENAME));
    }

    private void FetchCalendarType(CalendarType type, Action onComplete)
    {
        if (!IsCalendarTypeLoaded(type))
            Scenes.Load(CalendarTypeToSceneName(type), LoadSceneMode.Additive,
                (Scene scene) =>
                {
                    FetchCalendarTypeFromScene(type, scene);
                    if (onComplete != null)
                        onComplete();
                });
        else
        {
            FetchCalendarTypeFromScene(type, Scenes.GetActive(CalendarTypeToSceneName(type)));
            if (onComplete != null)
                onComplete();
        }
    }

    private void FetchCalendarTypeFromScene(CalendarType type, Scene scene)
    {
        switch (type)
        {
            case CalendarType.Scroll:
                scrollCalendar = scene.FindRootObject<CalendarScroll_Controller>();
                break;
            case CalendarType.Grid:
                gridCalendar = scene.FindRootObject<CalendarGrid_Controller>();
                break;
        }
    }

    public static bool IsCalendarTypeLoaded(CalendarType type)
    {
        return Scenes.IsActiveOrBeingLoaded(CalendarTypeToSceneName(type));
    }

    public static string CalendarTypeToSceneName(CalendarType type)
    {
        switch (type)
        {
            case CalendarType.Scroll:
                return CalendarScroll_Controller.SCENENAME;
            case CalendarType.Grid:
                return CalendarGrid_Controller.SCENENAME;
            default:
                return "";
        }
    }
}
