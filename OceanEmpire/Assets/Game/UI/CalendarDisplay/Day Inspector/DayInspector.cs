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
    [SerializeField] SceneInfo exerciceSelectionWindow;

    [Header("Components"), SerializeField] Button exitButton;
    [SerializeField] RectTransform container;
    [SerializeField] CanvasGroupBehaviour blackBG;
    [SerializeField] RectTransform schedulesContainer;
    [SerializeField] Text nothingPlannedText;
    [SerializeField] Button scheduleButton;

    [Header("Header"), SerializeField] Text dateText;
    [SerializeField] Text dayOfWeekText;


    [Header("Window Animation"), SerializeField] float moveDuration;
    [SerializeField] Ease hideEase;
    [SerializeField] Ease showEase;
    [SerializeField] Vector2 hiddenAnchoredPos;
    [SerializeField] Vector2 shownAnchoredPos;

    List<GameObject> trash = new List<GameObject>();

    Calendar.Day day;

    private void Awake()
    {
        HideInstant();
        exitButton.onClick.AddListener(Hide);
        scheduleButton.onClick.AddListener(Plan);
    }

    void Plan()
    {
        Scenes.LoadAsync(exerciceSelectionWindow, (scene) =>
        {
            var window = scene.FindRootObject<SelectionScript>();
            window.Init();
            window.SelectionEvent += delegate(PossibleExercice.PlannedExercice plannedExercice) {
                Scenes.LoadAsync(askForTimeWindow, (scene2) =>
                {
                    var window1 = scene2.FindRootObject<AskForTimeWindow>();
                    window1.currentPlannedExercice = plannedExercice;
                    window1.AnswerEvent += ScheduleNewBonifiedTime;
                });
            };
        });
    }

    private void ScheduleNewBonifiedTime(bool confirmed, int hours, int minutes, PossibleExercice.PlannedExercice plannedExercice)
    {
        if (!confirmed)
            return;

        //TODO : TROUVER UNE FACON D'INTEGRER PLANNED EXERCICE ICI (D'HABITUDE IL VA DANS BONIFIED TIME)
        Debug.Log("EXERCICE EST ICI : " + plannedExercice.type);
        DateTime date = new DateTime(day.year, day.monthOfYear, day.dayOfMonth, hours, minutes, 0);
        TimeSlot timeSlot = new TimeSlot(date, ScheduledBonus.DefaultDuration());
        ScheduledBonus bonifiedTime = new ScheduledBonus(timeSlot, ScheduledBonus.DefaultBonus());

        if (!Calendar.instance.AddBonifiedTime(bonifiedTime))
        {
            MessagePopup.DisplayMessage("La plage horaire est déjà occupé.");
        }
        else
        {
            RefreshSchedules();
        }
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
        RefreshSchedules();
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

    public void RefreshSchedules()
    {
        EmptyTrash();

        bool enableNothingPlannedText = true;
        var bonifiedTimes = Calendar.instance.GetAllBonifiedTimesStartingOn(day);

        if (bonifiedTimes.Count > 0)
        {
            enableNothingPlannedText = false;
            for (int i = 0; i < bonifiedTimes.Count; i++)
            {
                var scheduleDisplay = schedulePrefab.DuplicateGO(schedulesContainer);
                var roundedStrength = bonifiedTimes[i].bonus.ticketMultiplier.Rounded(1);
                scheduleDisplay.FillContent(bonifiedTimes[i].timeSlot,
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
