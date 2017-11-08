using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarScroll_Day : MonoBehaviour
{
    [Header("Links")]
    public Text monthText;
    public Image bg;
    public Text dayText;

    [Header("Settings")]
    public Color todayColor = Color.white;

    private Color notTodayColor;
    private Calendar.Day day;

    void Awake()
    {
        notTodayColor = bg.color;
    }
    
    public void Fill(Calendar.Day day, bool isToday)
    {
        this.day = day;
        dayText.text = day.dayOfMonth.ToString();
        monthText.text = Calendar.GetMonthAbbreviation(day.monthOfYear);
        SetTodayVisuals(isToday);
    }

    private void SetTodayVisuals(bool state)
    {
        if (state)
        {
            bg.color = todayColor;
        }
        else
        {
            bg.color = notTodayColor;
        }
    }

    public Calendar.Day GetDay() { return day; }
}
