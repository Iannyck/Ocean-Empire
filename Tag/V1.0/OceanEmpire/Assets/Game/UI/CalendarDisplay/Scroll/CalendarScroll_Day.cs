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
    public Button button;

    [Header("Settings")]
    public Color todayColor = Color.white;

    public delegate void ElementEvent(CalendarScroll_Day day);
    public ElementEvent onClick;

    private Color notTodayColor;
    private Calendar.Day day;

    void Awake()
    {
        notTodayColor = bg.color;
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (onClick != null)
            onClick(this);
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
