using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarScroll_Day : MonoBehaviour
{
    [Header("Components")]
    public Text monthText;
    public Text dayOfTheWeekText;
    public Image bg;
    public Text dayOfTheMonthText;
    public Button button;
    public RectTransform[] potentialSchedules;
    public Image threeDots;

    [Header("Prefabs")]
    public CalendarScroll_Schedule bonifiedTimePrefab;

    [Header("Settings")]
    public Color todayColor = Color.white;

    public delegate void ElementEvent(CalendarScroll_Day day);
    public ElementEvent onClick;

    private Color notTodayColor;
    private Calendar.Day day;
    private List<GameObject> trash = new List<GameObject>();

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
        dayOfTheMonthText.text = day.dayOfMonth.ToString();
        monthText.text = Calendar.GetMonthAbbreviation(day.monthOfYear);
        dayOfTheWeekText.text = Calendar.GetDayOfTheWeekName(day.dayOfWeek);

        RefreshContent();

        SetTodayVisuals(isToday);
    }

    public void RefreshContent()
    {
        EmptyTrash();

        var bonifiedTimesToday = Calendar.instance.GetAllBonifiedTimesOn(day);

        bool enableThreeDots = false;
        for (int i = 0; i < bonifiedTimesToday.Count; i++)
        {
            if (i >= potentialSchedules.Length)
            {
                enableThreeDots = true;
                break;
            }
            var bonifiedTimeUI = bonifiedTimePrefab.DuplicateGO(potentialSchedules[i]);
            bonifiedTimeUI.FillContent(
                bonifiedTimesToday[i].timeslot,
                "BONUS X" + bonifiedTimesToday[i].bonusStrength.Rounded(1));
            trash.Add(bonifiedTimeUI.gameObject);
        }

        threeDots.enabled = enableThreeDots;
        //temp.enabled = bonifiedTimesToday.Count > 0;
    }

    private void EmptyTrash()
    {
        for (int i = 0; i < trash.Count; i++)
        {
            if (trash[i] != null)
                Destroy(trash[i]);
        }
        trash.Clear();
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
