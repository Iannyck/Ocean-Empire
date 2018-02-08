using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarScroll_Scroller : MonoBehaviour
{
    [Header("Links")]
    public CanvasGroupBehaviour backToTopButton;
    public CanvasGroupBehaviour backToBottomButton;
    public ScrollRect scrollRect;
    public RectTransform container;
    public List<CalendarScroll_Day> days;

    int cumulativeRewinds;

    void Awake()
    {
        backToTopButton.HideInstant();
        backToBottomButton.HideInstant();
    }

    public void Fill(List<Calendar.Day> list)
    {
        Calendar.Day today = new Calendar.Day(DateTime.Now);

        int c = Mathf.Min(list.Count, days.Count);
        for (int i = 0; i < c; i++)
        {
            days[i].Fill(list[i], list[i] == today);
        }

        scrollRect.verticalNormalizedPosition = 1;
        cumulativeRewinds = 0;
        UpdateResetButtons();
    }

    public void RefreshContent()
    {
        for (int i = 0; i < days.Count; i++)
        {
            days[i].RefreshContent();
        }
    }

    public void Rewind(int amount)
    {
        bool bottomToTop = amount > 0;
        int c = amount.Abs();
        for (int i = 0; i < c; i++)
        {
            SetSiblingAndFill(!bottomToTop);
        }

        cumulativeRewinds += amount;
        UpdateResetButtons();
    }

    void UpdateResetButtons()
    {
        bool toTopState = cumulativeRewinds < -7;
        bool toBottomState = cumulativeRewinds > 7;

        if (backToBottomButton.IsShown != toBottomState)
        {
            if (toBottomState)
                backToBottomButton.Show();
            else
                backToBottomButton.Hide();
        }

        if (backToTopButton.IsShown != toTopState)
        {
            if (toTopState)
                backToTopButton.Show();
            else
                backToTopButton.Hide();
        }
    }

    private void SetSiblingAndFill(bool putLast)
    {
        int childIndex = putLast ? 0 : days.Count - 1;
        CalendarScroll_Day dayItem = days[childIndex];
        Calendar.Day day;

        if (putLast)
        {
            day = new Calendar.Day(days.Last().GetDay().ToDateTime().AddDays(1));
            dayItem.transform.SetAsLastSibling();
            days.RemoveAt(childIndex);
            days.Add(dayItem);
        }
        else
        {
            day = new Calendar.Day(days[0].GetDay().ToDateTime().AddDays(-1));
            dayItem.transform.SetAsFirstSibling();
            days.RemoveAt(childIndex);
            days.Insert(0, dayItem);
        }

        dayItem.Fill(day, day == new Calendar.Day(DateTime.Now));
    }
}
