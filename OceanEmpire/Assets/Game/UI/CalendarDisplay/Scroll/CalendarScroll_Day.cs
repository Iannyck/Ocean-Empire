using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarScroll_Day : MonoBehaviour
{
    [Header("Components"), SerializeField] Text monthText;
    [SerializeField] Text dayOfTheWeekText;
    [SerializeField] Image bg;
    [SerializeField] Text dayOfTheMonthText;
    [SerializeField] Button button;
    [SerializeField] RectTransform[] potentialSchedules;
    [SerializeField] Image threeDots;

    [Header("Prefabs"), SerializeField]
    CalendarScroll_Schedule bonifiedTimePrefab;

    [Header("Settings"), SerializeField] Color todayColor = Color.white;
    [SerializeField] Text todayText;

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

        var schedulesToday = Calendar.instance.GetAllSchedulesStartingOn(day);

        bool enableThreeDots = false;
        for (int i = 0; i < schedulesToday.Count; i++)
        {
            if (i >= potentialSchedules.Length)
            {
                enableThreeDots = true;
                break;
            }
            var scheduleUI = bonifiedTimePrefab.DuplicateGO(potentialSchedules[i]);
            scheduleUI.FillContent(schedulesToday[i]);
            //bonifiedTimeUI.FillContent(
            //    schedulesToday[i].timeSlot,
            //    "BONUS X" + schedulesToday[i].bonus.ticketMultiplier.Rounded(1));
            trash.Add(scheduleUI.gameObject);
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
        todayText.enabled = state;
        bg.color = state ? todayColor : notTodayColor;
    }

    public Calendar.Day GetDay() { return day; }
}
