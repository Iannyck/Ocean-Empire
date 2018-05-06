using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantExerciseChoice_Item : MonoBehaviour
{
    [Header("UI Links")]
    //public Text taskText;
    public Text ticketText;
    public Text averageDurationText;
    public Text realIntervalText;
    public Text levelText;

    [Header("Data")]
    public Task assignedTask;
    public Action<InstantExerciseChoice_Item> onClick;

    void Awake()
    {
        FillContent(assignedTask);
    }

    public void FillContent(Task task)
    {
        assignedTask = task;
        //taskText.text = "<size=18>Marche de </size><color=#f>" + task.minDuration + " à " + task.maxDuration + " min</color>";

        if (ticketText)
            ticketText.text = task.ticketReward.ToString();
        if (averageDurationText)
            averageDurationText.text = task.advertisedDuration + " min";
        if (realIntervalText)
            realIntervalText.text = task.minDuration + " min\nà\n" + task.maxDuration + " min";
        if (levelText)
            levelText.text = "Niv " + task.level;
    }

    public void Click()
    {
        if (onClick != null)
            onClick(this);
    }

    void OnValidate()
    {
        if (assignedTask != null)
            FillContent(assignedTask);
    }
}
