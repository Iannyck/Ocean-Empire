using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailureAsk : MonoBehaviour
{
    [Header("Windows")]
    public WindowAnimation choiceWindow;
    public WindowAnimation adviceWindow;

    [Header("Schedule UI")]
    public InstantExerciseChoice_Item taskUI;
    public Text timeslotText;
    public Text dateText;

    [Header("Choices")]
    public Transform choiceContainer;

    Action onReportFilled;
    PlannedExerciceRewarder.Report report;

    void Start()
    {
        var choices = choiceContainer.GetComponentsInChildren<ChoiceButton>();
        foreach (var choice in choices)
        {
            choice.OnClick = OnChoiceSelected;
        }
    }

    public void FillContent(PlannedExerciceRewarder.Report report, Action onReportFilled)
    {
        this.onReportFilled = onReportFilled;
        this.report = report;

        var start = report.schedule.timeSlot.start;
        var end = report.schedule.timeSlot.end;

        taskUI.FillContent(report.schedule.task);
        dateText.text = "" +
            Calendar.GetDayOfTheWeekName(start.DayOfWeek) + ", " +
            start.Day + " " +
            Calendar.GetMonthAbbreviation(start.Month);
        timeslotText.text = "Entre <color=#f>" + start.ToCondensedTimeOfDayString() + "</color>\net <color=#f>"
            + end.ToCondensedTimeOfDayString() + "</color>";
    }

    void OnChoiceSelected(ChoiceButton choiceButton)
    {
        var advice = choiceButton.choice.advice;

        report.playerConclusion = choiceButton.choice.choiceText;

        // Y a-t-il un conseil ?
        if (advice == "")
        {
            choiceWindow.Close(UnloadScene);
        }
        else
        {
            choiceWindow.Close(delegate ()
            {
                adviceWindow.GetComponent<FailureAdvice>().Display(choiceButton.choice.advice, UnloadScene);
            });
        }
    }

    void UnloadScene()
    {
        if (onReportFilled != null)
            onReportFilled();

        // Unload scene
        Scenes.UnloadAsync(gameObject.scene);
    }
}
