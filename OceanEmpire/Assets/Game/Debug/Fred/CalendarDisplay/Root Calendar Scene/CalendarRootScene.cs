using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using System;
using UnityEngine.SceneManagement;

public class CalendarRootScene : MonoBehaviour
{
    public const string SCENENAME = "CalendarRootScene";

    public enum CalendarType { Scroll = 0, Grid = 1 }
    public CalendarType defaultType = CalendarType.Scroll;

    private void Start()
    {

    }

    private void LoadCalendarType(CalendarType type, Action action)
    {
        //Scenes.Load(CalendarTypeToSceneName(type), )
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
