using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CalendarGrid_Controller : CanvasGroupBehaviour
{
    public const string SCENENAME = "CalendarGrid";

    public int currentWeekIndex = 0;
    public CalendarGrid_Scroller scroller;

    //private Calendar calendar;

    private void Awake()
    {
        //Add 'onClick' listeners
        List<CalendarGrid_Week> weeks = scroller.weeks;
        for (int i = 0; i < weeks.Count; i++)
        {
            for (int j = 0; j < weeks[i].days.Length; j++)
            {
                weeks[i].days[j].onClick = OnDayClick;
            }
        }

        HideInstant();
    }

    private void OnDayClick(CalendarGrid_Day day)
    {

    }

    public void Fill(Calendar calendar)
    {
        //this.calendar = calendar;
        int weekIndex = -1 - currentWeekIndex;
        DateTime now = DateTime.Now;

        List<CalendarGrid_Week> weeks = scroller.weeks;
        for (int i = 0; i < weeks.Count; i++)
        {
            weeks[i].Fill(Calendar.GetDaysOfWeek(now.AddWeeks(weekIndex)));
            weekIndex++;
        }
        scroller.CenterScroller();
    }

    public void GoForwardOneWeek()
    {
        scroller.ScrollDown(SnapAndUpdate);
    }
    public void GoBackwardOneWeek()
    {
        scroller.ScrollUp(SnapAndUpdate);
    }

    void SnapAndUpdate()
    {
        List<CalendarGrid_Week> weeks = scroller.weeks;

        //Build new weeks
        if (scroller.scroller.verticalNormalizedPosition < 0.5f)
        {
            //On replace le premier en dernier
            CalendarGrid_Week newWeek = scroller.PutTopAtBottom();

            //Construit celui en bas
            newWeek.Fill(
                Calendar.GetDaysOfWeek(
                    weeks[weeks.Count - 2].GetCalendarDay().ToDateTime().AddWeeks(1)));
        }
        else
        {
            //On replace le dernier en premier
            CalendarGrid_Week newWeek = scroller.PutBottomAtTop();

            //Construit celui en haut
            newWeek.Fill(
                Calendar.GetDaysOfWeek(
                    weeks[1].GetCalendarDay().ToDateTime().AddWeeks(-1)));
        }
        scroller.CenterScroller();
    }
}
