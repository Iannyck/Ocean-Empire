using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCC.Math.Graph;
using Questing;
using System;
using UnityEditor;
using UnityEngine.SceneManagement;

public class TestScript_Fred : MonoBehaviour
{
    public int questBuilderIndex = 0;
    public FishingFrenzyCategory frenzyCategory;
    public UnityEngine.Object[] questBuilders;

    void Start()
    {
        PersistentLoader.LoadIfNotLoaded();
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            IQuestBuilder questBuilder = questBuilders[questBuilderIndex] as IQuestBuilder;
            QuestManager.Instance.AddQuest(questBuilder.BuildQuest(DateTime.Now));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            QuestManager.Instance.RemoveQuest(QuestManager.Instance.ongoingQuests[0]);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            frenzyCategory.MakeAvailable();
            Debug.Log("We've unlocked the fishing frenzy in the shop");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CalendarRequest.Settings settings = new CalendarRequest.Settings()
            {
                windowHeaderTitle = "Quel jour?",
                scheduleButtonText = "Plannifier l'exercice"
            };
            var handle = CalendarRequest.LaunchRequest(settings);

            handle.onTimePicked = (date) =>
            {
                Debug.Log("DateTime picked: " + date);
                TimeSlot timeSlot = new TimeSlot(date, ScheduledBonus.DefaultDuration());
                ScheduledBonus bonifiedTime = new ScheduledBonus(timeSlot, ScheduledBonus.DefaultBonus());

                if (!Calendar.instance.AddSchedule(bonifiedTime))
                {
                    MessagePopup.DisplayMessage("La plage horaire est déjà occupé.");
                }
                else
                {
                    handle.CloseWindow();
                }
            };
            handle.onWindowLoaded = (scene) => Debug.Log("On Window Loaded: " + scene.name);
            handle.onWindowIntroComplete = () => Debug.Log("On Window Intro Complete");
            handle.onWindowExitStarted = () => Debug.Log("On Window Exit Started");
            handle.onWindowExitComplete = () => Debug.Log("On Window Exit Complete");
        }
    }
}