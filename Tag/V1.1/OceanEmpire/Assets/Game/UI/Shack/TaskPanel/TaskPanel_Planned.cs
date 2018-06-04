using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel_Planned : MonoBehaviour, ITaskPanelState
{
    public Text dateText;
    public TaskPanel taskPanel;

    // On réutilise des bout de code, hehehe \(• ◡ •)/
    public InstantExerciseChoice_Item uiDisplay;

    private Schedule schedule;

    public void Enter(Action onComplete)
    {
        gameObject.SetActive(true);
        if (onComplete != null)
            onComplete();
    }

    public void Exit(Action onComplete)
    {
        gameObject.SetActive(false);
        if (onComplete != null)
            onComplete();
    }

    public void FillContent(object data)
    {
        schedule = data as Schedule;
        if (schedule == null)
        {
            Debug.LogError("Could not fill TaskPanel_Planned with data");
            return;
        }

        uiDisplay.FillContent(schedule.task);

        var timeslot = schedule.timeSlot;
        var start = timeslot.start;

        dateText.text = "" +
            Calendar.GetDayOfTheWeekName(start.DayOfWeek) + ", " +
            start.Day + " " +
            Calendar.GetMonthAbbreviation(start.Month) + "  -  <color=#f>Entre " +
            timeslot.ToCondensedDayOfTimeString() + "</color>";
    }

    public void OfferToStartNow()
    {
        if (Calendar.instance.CouldMoveScheduleToNow(schedule))
        {
            Debug.Log("Fenetre de confirmation");

            YesNoWindow.AskYesNoQuestion("Débuter l'exercice maintenant?", (yes) =>
            {
                if (yes)
                {
                    Debug.Log("Moved early: " + Calendar.instance.MoveScheduleToNow(schedule));
                    PlannedExerciceRewarder.Instance.ForceAnalyseCheck();
                }
            });
        }
    }
}
