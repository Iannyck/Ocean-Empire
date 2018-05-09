using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletionWindow : WindowAnimation
{
    public InstantExerciseChoice_Item taskUI;
    public Text dateText;
    public Text timeSlotText;
    public Text rewardText;

    Action onComplete;

    public void FillContent(PlannedExerciceRewarder.Report report, Action onComplete)
    {
        this.onComplete = onComplete;

        var start = report.schedule.timeSlot.start;
        var end = report.schedule.timeSlot.end;

        taskUI.FillContent(report.schedule.task);
        dateText.text = "" +
            Calendar.GetDayOfTheWeekName(start.DayOfWeek) + ", " +
            start.Day + " " +
            Calendar.GetMonthAbbreviation(start.Month);
        timeSlotText.text = "Entre <color=#f>" + start.ToCondensedTimeOfDayString() + "</color>\net <color=#f>"
            + end.ToCondensedTimeOfDayString() + "</color>";

        rewardText.text = "+ " + report.schedule.task.ticketReward;
    }

    public void ContinueClick()
    {
        if (onComplete != null)
            onComplete();

        Close();
    }
}
