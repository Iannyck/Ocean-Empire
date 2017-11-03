using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class CalendarScroll_Controller : MonoBehaviour
{
    public int currentWeekIndex = 0;
    public CalendarScroll_Scroller scroller;
    
    private Calendar calendar;

    private void Awake()
    {
        //Add 'onClick' listeners
        List<CalendarScroll_Week> weeks = scroller.weeks;
        for (int i = 0; i < weeks.Count; i++)
        {
            for (int j = 0; j < weeks[i].days.Length; j++)
            {
                weeks[i].days[j].onClick = OnDayClick;
            }
        }
    }

    private void OnDayClick(CalendarScroll_Day day)
    {

    }

    public void Fill(Calendar calendar)
    {
        this.calendar = calendar;
        int weekIndex = -1 - currentWeekIndex;
        DateTime now = DateTime.Now;

        List<CalendarScroll_Week> weeks = scroller.weeks;
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
        List<CalendarScroll_Week> weeks = scroller.weeks;

        //Build new weeks
        if (scroller.scroller.verticalNormalizedPosition < 0.5f)
        {
            //On replace le premier en dernier
            CalendarScroll_Week newWeek = scroller.PutTopAtBottom();

            //Construit celui en bas
            newWeek.Fill(
                Calendar.GetDaysOfWeek(
                    weeks[weeks.Count - 2].GetCalendarDay().GetAnchorDateTime().AddWeeks(1)));
        }
        else
        {
            //On replace le dernier en premier
            CalendarScroll_Week newWeek = scroller.PutBottomAtTop();

            //Construit celui en haut
            newWeek.Fill(
                Calendar.GetDaysOfWeek(
                    weeks[1].GetCalendarDay().GetAnchorDateTime().AddWeeks(-1)));
        }
        scroller.CenterScroller();
    }
}
