using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using System;
using UnityEngine.SceneManagement;

public class CalendarRootScene : MonoBehaviour
{
    public const string SCENENAME = "CalendarRootScene";

    [ReadOnly]
    public CalendarGrid_Controller gridCalendar;
    [ReadOnly]
    public CalendarScroll_Controller scrollCalendar;

    public enum CalendarType { Scroll = 0, Grid = 1 }
    public CalendarType defaultType = CalendarType.Scroll;

    private void Start()
    {
        MasterManager.Sync(() =>
        {
            InitQueue queue = new InitQueue(OnCalendarsLoaded);

            if (!IsCalendarTypeLoaded(CalendarType.Scroll))
                FetchCalendarType(CalendarType.Scroll, queue.Register());

            if (!IsCalendarTypeLoaded(CalendarType.Grid))
                FetchCalendarType(CalendarType.Grid, queue.Register());

            queue.MarkEnd();
        });
    }

    private void OnCalendarsLoaded()
    {
        switch (defaultType)
        {
            case CalendarType.Scroll:
                if (!scrollCalendar.IsShown)
                    scrollCalendar.Show();
                break;
            case CalendarType.Grid:
                if (!gridCalendar.IsShown)
                    gridCalendar.Show();
                break;
        }
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
        return Scenes.Exists(CalendarTypeToSceneName(type));
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
