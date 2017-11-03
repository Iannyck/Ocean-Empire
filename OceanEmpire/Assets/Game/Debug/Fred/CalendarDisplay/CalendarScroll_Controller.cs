using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class CalendarScroll_Controller : MonoBehaviour
{
    public ScrollRect scroller;
    public RectTransform content;
    public int currentWeekIndex = 0;

    [Header("List")]
    public List<CalendarScroll_Week> weeks = new List<CalendarScroll_Week>();


    [Header("Animation")]
    public float scrollDuration;
    public Ease scrollEase;

    public bool IsAnimating() { return tween != null && tween.IsActive(); }

    private Tween tween;
    private Calendar calendar;

    public void Fill(Calendar calendar)
    {
        this.calendar = calendar;
        int weekIndex = -1 - currentWeekIndex;
        DateTime now = DateTime.Now;
        for (int i = 0; i < weeks.Count; i++)
        {
            weeks[i].Fill(Calendar.GetDaysOfWeek(now.AddWeeks(weekIndex)));
            weekIndex++;
        }
        CenterScroller();
        //UpdateMonthLabels();
    }

    public void GoForwardOneWeek()
    {
        if (IsAnimating())
            return;
        KillTween();
        tween = scroller.DOVerticalNormalizedPos(1, scrollDuration).SetEase(scrollEase).OnComplete(SnapAndUpdate);
    }
    public void GoBackwardOneWeek()
    {
        if (IsAnimating())
            return;
        KillTween();
        tween = scroller.DOVerticalNormalizedPos(0, scrollDuration).SetEase(scrollEase).OnComplete(SnapAndUpdate);
    }

    void KillTween()
    {
        if (tween != null && tween.IsActive())
        {
            tween.Kill();
            tween = null;
        }
    }

    void SnapAndUpdate()
    {
        //Build new weeks
        if (scroller.verticalNormalizedPosition < 0.5f)
        {
            //On replace le premier en dernier
            CalendarScroll_Week transitioningWeek = weeks[0];
            weeks.RemoveAt(0);
            weeks.Add(transitioningWeek);
            transitioningWeek.transform.SetAsLastSibling();

            //Construit celui en bas
            transitioningWeek.Fill(
                Calendar.GetDaysOfWeek(
                    weeks[weeks.Count - 2].GetCalendarDay().GetAnchorDateTime().AddWeeks(1)));
        }
        else
        {
            //On replace le dernier en premier
            CalendarScroll_Week transitioningWeek = weeks.Last();
            weeks.RemoveLast();
            weeks.Insert(0, transitioningWeek);
            transitioningWeek.transform.SetAsFirstSibling();

            //Construit celui en haut
            transitioningWeek.Fill(
                Calendar.GetDaysOfWeek(
                    weeks[1].GetCalendarDay().GetAnchorDateTime().AddWeeks(-1)));
        }
        CenterScroller();
        //UpdateMonthLabels();
    }

    void UpdateMonthLabels()
    {
        weeks[0].days[0].ShowMonthLabel = false;
        weeks[1].days[0].ShowMonthLabel = true;
        weeks[2].days[0].ShowMonthLabel = false;
    }

    void CenterScroller()
    {
        scroller.verticalNormalizedPosition = 0.5f;
    }
}
