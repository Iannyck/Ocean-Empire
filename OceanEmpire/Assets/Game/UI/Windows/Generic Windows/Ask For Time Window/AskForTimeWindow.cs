using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OceanEmpire.AskForTimeWindow;
using System;
using UnityEngine.UI;

public class AskForTimeWindow : MonoBehaviour
{
    [SerializeField] private WindowAnimation windowAnimation;

    [SerializeField] private TimeText hoursDisplay;
    [SerializeField] private TimeText minutesDisplay;
    [SerializeField] private int middleElementIndex;

    [SerializeField, Header("Buttons")] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    [SerializeField, Header("Texts")] private Text confirmButtonText;
    [SerializeField] private Text headerText;

    public delegate void AnswerHandler(bool confirmed, int hours, int minutes, PossibleExercice.PlannedExercice plannedExercice);
    
    /// <summary>
    /// Callback {
    /// <para /> bool : player has confirmed
    /// <para />int : hours
    /// <para />int : minutes
    /// <para />}
    /// </summary>
    public event AnswerHandler AnswerEvent;

    public PossibleExercice.PlannedExercice currentPlannedExercice;

    public void SetHeader(string text)
    {
        headerText.text = text;
    }

    public bool CanCancel { set { cancelButton.gameObject.SetActive(value); } }

    public void SetButtonText(string text)
    {
        confirmButtonText.text = text;
    }

    private void Awake()
    {
        confirmButton.onClick.AddListener(Confirm);
        cancelButton.onClick.AddListener(Cancel);
    }

    private void Confirm()
    {
        windowAnimation.Close();

        int hours = hoursDisplay.GetNumberAt(middleElementIndex);
        int minutes = minutesDisplay.GetNumberAt(middleElementIndex);

        if (AnswerEvent != null)
            AnswerEvent(true, hours, minutes, currentPlannedExercice);
    }

    private void Cancel()
    {
        windowAnimation.Close();

        if (AnswerEvent != null)
            AnswerEvent(false, -1, -1, currentPlannedExercice);
    }
}
