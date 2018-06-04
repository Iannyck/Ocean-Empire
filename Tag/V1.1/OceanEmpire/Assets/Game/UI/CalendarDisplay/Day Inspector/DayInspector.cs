using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class DayInspector : MonoBehaviour
{
    public const string SCENENAME = "DayInspector";

    [Header("Assets"), SerializeField] DayInspector_Schedule schedulePrefab;
    [SerializeField] SceneInfo askForTimeWindow;

    [Header("Components"), SerializeField] Button exitButton;
    [SerializeField] RectTransform container;
    [SerializeField] CanvasGroupBehaviour blackBG;
    [SerializeField] RectTransform schedulesContainer;
    [SerializeField] Text nothingPlannedText;
    [SerializeField] Button scheduleButton;
    [SerializeField] Text scheduleButtonText;

    [Header("Header"), SerializeField] Text dateText;
    [SerializeField] Text dayOfWeekText;


    [Header("Window Animation"), SerializeField] float moveDuration;
    [SerializeField] Ease hideEase;
    [SerializeField] Ease showEase;
    [SerializeField] Vector2 hiddenAnchoredPos;
    [SerializeField] Vector2 shownAnchoredPos;

    List<GameObject> trash = new List<GameObject>();

    Calendar.Day day;

    public CalendarRequest.RequestHandle RequestHandle { get; set; }
    public bool IsVisible { get; private set; }
    public bool CanPlanInPast { get; private set; }

    private void Awake()
    {
        HideInstant();
        exitButton.onClick.AddListener(Hide);
        scheduleButton.onClick.AddListener(Plan);
    }

    void Plan()
    {
        Scenes.LoadAsync(askForTimeWindow, (scene) =>
        {
            var window = scene.FindRootObject<AskForTimeWindow>();
            window.AnswerEvent += OnTimeSelected;
        });
    }

    private void OnTimeSelected(bool confirmed, int hours, int minutes)
    {
        // Le joueur a clické 'Cancel' ?
        if (!confirmed)
            return;

        DateTime date = new DateTime(day.year, day.monthOfYear, day.dayOfMonth, hours, minutes, 0);


        if (!CanPlanInPast)
        {
            DateTime now = DateTime.Now;
            if (date < now)
            {
                MessagePopup.DisplayMessage("Vous ne pouvez pas planifer dans le passé.");
                return;
            }
        }


        if (RequestHandle != null)
            RequestHandle.onTimePicked(date);

        RefreshSchedules();
    }

    public void ShowAndFill(Calendar.Day day) { ShowAndFill(day, null); }
    public void ShowAndFill(Calendar.Day day, TweenCallback onComplete)
    {
        IsVisible = true;
        Fill(day);
        blackBG.Show();

        container.DOKill();
        container.DOAnchorPos(shownAnchoredPos, moveDuration).SetEase(showEase).OnComplete(onComplete);
    }

    public void Fill(Calendar.Day day)
    {
        this.day = day;

        RefreshHeader();
        RefreshSchedules();
    }

    public void Hide() { Hide(null); }
    public void Hide(TweenCallback onComplete)
    {
        IsVisible = false;
        blackBG.Hide();

        container.DOKill();
        container.DOAnchorPos(hiddenAnchoredPos, moveDuration).SetEase(hideEase).OnComplete(onComplete);
    }

    public void ShowInstant()
    {
        IsVisible = true;
        blackBG.ShowInstant();

        container.DOKill();
        container.anchoredPosition = shownAnchoredPos;
    }

    public void HideInstant()
    {
        IsVisible = false;
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

    public void RefreshSchedules()
    {
        EmptyTrash();

        scheduleButton.gameObject.SetActive(CanPlanInPast || day.ToDateTime() > DateTime.Now.AddDays(-1));

        bool enableNothingPlannedText = true;
        var schedules = Calendar.instance.GetAllSchedulesStartingOn(day);

        if (schedules.Count > 0)
        {
            enableNothingPlannedText = false;
            for (int i = 0; i < schedules.Count; i++)
            {
                var scheduleDisplay = schedulePrefab.DuplicateGO(schedulesContainer);
                //var roundedStrength = schedules[i].bonus.ticketMultiplier.Rounded(1);

                scheduleDisplay.FillContent(schedules[i]);
                //scheduleDisplay.FillContent(schedules[i].timeSlot,
                //    "BONUS X" + roundedStrength
                //    , "Faire de l'exercice vous rapportera " + roundedStrength + "x plus de tickets!");
                trash.Add(scheduleDisplay.gameObject);
            }
        }

        nothingPlannedText.enabled = enableNothingPlannedText;
    }

    public void ApplySettings(CalendarRequest.Settings requestSettings)
    {
        scheduleButtonText.text = requestSettings.scheduleButtonText;
        CanPlanInPast = requestSettings.canPlanInPast;
        RefreshSchedules();
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
