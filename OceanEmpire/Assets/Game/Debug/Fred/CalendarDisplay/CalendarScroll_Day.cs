using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarScroll_Day : MonoBehaviour
{
    public Text dayNumber;
    public Text monthLabel;
    public Image image;
    public Color stdColor = Color.white;
    public Color todayColor = Color.blue;
    public delegate void DayEvent(CalendarScroll_Day day);
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
                monthLabel.text = GetMonthToLabel();
            }
        }
    }
    private string GetMonthToLabel()
    {
        switch (data.monthOfYear)
        {
            case 1:
                return "Jan";
            case 2:
                return "F\u00E9v";
            case 3:
                return "Mar";
            case 4:
                return "Avr";
            case 5:
                return "Mai";
            case 6:
                return "Jun";
            case 7:
                return "Jul";
            case 8:
                return "Ao\u00FB";
            case 9:
                return "Sep";
            case 10:
                return "Oct";
            case 11:
                return "Nov";
            case 12:
                return "D\u00E9c";
            default:
                return "NaM";
        }
    }
    public Calendar.Day GetCalendarDay()
    {
        return data;
    }
}