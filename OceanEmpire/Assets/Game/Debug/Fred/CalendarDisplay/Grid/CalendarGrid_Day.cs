using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarGrid_Day : MonoBehaviour
{
    public Text dayNumber;
    public Text monthLabel;
    public Image image;
    public Color stdColor = Color.white;
    public Color todayColor = Color.blue;
    public delegate void DayEvent(CalendarGrid_Day day);
    public DayEvent onClick;

    private Calendar.Day data;

    void Awake()
    {
        ShowMonthLabel = false;
    }

    public void Fill(Calendar.Day day, bool isToday)
    {
        data = day;
        dayNumber.text = day.dayOfMonth.ToString();
        image.color = isToday ? todayColor : stdColor;

        ShowMonthLabel = day.dayOfWeek == System.DayOfWeek.Sunday;
    }

    public void Click()
    {
        if (onClick != null)
            onClick(this);
    }

    public bool ShowMonthLabel
    {
        get { return monthLabel.enabled; }
        set
        {
            monthLabel.enabled = value;
            if (monthLabel.enabled)
            {
                monthLabel.text = Calendar.GetMonthAbbreviation(data.monthOfYear);
            }
        }
    }

    public Calendar.Day GetCalendarDay()
    {
        return data;
    }
}