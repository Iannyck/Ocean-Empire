using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCC.Math.Graph;
using Questing;
using System;
using UnityEditor;

public class TestScript_Fred : MonoBehaviour
{
    public int questBuilderIndex = 0;
    public FishingFrenzyCategory frenzyCategory;
    public UnityEngine.Object[] questBuilders;

    void Start()
    {
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
    }
}