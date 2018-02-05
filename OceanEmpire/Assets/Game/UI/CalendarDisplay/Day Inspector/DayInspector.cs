using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class DayInspector : MonoBehaviour
{
    public const string SCENENAME = "DayInspector";

    [Header("Prefabs"), SerializeField] DayInspector_Schedule schedulePrefab;

    [Header("Components"), SerializeField] Button exitButton;
    [SerializeField] RectTransform container;
    [SerializeField] CanvasGroupBehaviour blackBG;
    [SerializeField] RectTransform schedulesContainer;
    [SerializeField] Text nothingPlannedText;

    [Header("Header"), SerializeField] Text dateText;
    [SerializeField] Text dayOfWeekText;


    [Header("Window Animation"), SerializeField] float moveDuration;
    [SerializeField] Ease hideEase;
    [SerializeField] Ease showEase;
    [SerializeField] Vector2 hiddenAnchoredPos;
    [SerializeField] Vector2 shownAnchoredPos;

    [ReadOnly] public CalendarRootScene root;

    List<GameObject> trash = new List<GameObject>();

    Calendar.Day day;

    private void Awake()
    {
        HideInstant();
        exitButton.onClick.AddListener(Hide);
    }

    public void ShowAndFill(Calendar.Day day) { ShowAndFill(day, null); }
    public void ShowAndFill(Calendar.Day day, TweenCallback onComplete)
    {
        Fill(day);
        blackBG.Show();

        container.DOKill();
        container.DOAnchorPos(shownAnchoredPos, moveDuration).SetEase(showEase).OnComplete(onComplete);
    }

    public void Fill(Calendar.Day day)
    {
        this.day = day;

        RefreshHeader();
        BuildSchedules();
    }

    public void Hide() { Hide(null); }
    public void Hide(TweenCallback onComplete)
    {
        blackBG.Hide();

        container.DOKill();
        container.DOAnchorPos(hiddenAnchoredPos, moveDuration).SetEase(hideEase).OnComplete(onComplete);
    }

    public void ShowInstant()
    {
        blackBG.ShowInstant();

        container.DOKill();
        container.anchoredPosition = shownAnchoredPos;
    }

    public void HideInstant()
    {
        blackBG.HideInstant();

        container.DOKill();
        container.anchoredPosition = hiddenAnchoredPos;
    }

    public void RefreshHeader()
    {
        char[] month = Calendar.GetMonthName(day.monthOfYear).ToCharArray();
        month[0] = Char.ToLower(month[0]);

        dateText.text = day.dayOfMonth + " " + new string(month) + " " + day.year;
        dayOfWeekText.text = Calendar.GetDayOfTheWeekName(day.dayOfWeek);
    }

    public void BuildSchedules()
    {
        EmptyTrash();

        bool enableNothingPlannedText = true;
        var bonifiedTimes = Calendar.instance.GetAllBonifiedTimesOn(day);

        if (bonifiedTimes.Count > 0)
        {
            enableNothingPlannedText = false;
            for (int i = 0; i < bonifiedTimes.Count; i++)
            {
                var scheduleDisplay = schedulePrefab.DuplicateGO(schedulesContainer);
                var roundedStrength = bonifiedTimes[i].bonusStrength.Rounded(1);
                scheduleDisplay.FillContent(bonifiedTimes[i].timeslot,
                    "BONUS X" + roundedStrength
                    , "Faire de l'exercice vous rapportera " + roundedStrength + "x plus de tickets!");
                trash.Add(scheduleDisplay.gameObject);
            }
        }

        nothingPlannedText.enabled = enableNothingPlannedText;
    }

    public void EmptyTrash()
    {
        for (int i = 0; i < trash.Count; i++)
        {
            Destroy(trash[i]);
        }
        trash.Clear();
    }
}
