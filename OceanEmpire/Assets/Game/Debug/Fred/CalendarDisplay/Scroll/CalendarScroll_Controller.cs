using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarScroll_Controller : CanvasGroupBehaviour
{
    public const string SCENENAME = "CalendarScroll";

    [Header("Links")]
    public CalendarScroll_Scroller scroller;

    [Header("Settings")]
    public int startingDayIndex = 1;

    [ReadOnly]
    public CalendarRootScene root;

    private void Awake()
    {
        HideInstant();
        List<CalendarScroll_Day> days = scroller.days;
        days.ForEach((x) => x.onClick += OnDayClick);
    }

    void Start()
    {
        Refill();
    }

    private void Refill()
    {
        scroller.Fill(Calendar.GetDaysFrom(DateTime.Now.AddDays(-startingDayIndex), scroller.days.Count));
    }

    public void OnDayClick(CalendarScroll_Day day)
    {
        root.dayInspector.Show(day.GetDay());
    }

    public void BackToTop()
    {
        Refill();
    }

    public void BackToBottom()
    {
        Refill();
    }
}
