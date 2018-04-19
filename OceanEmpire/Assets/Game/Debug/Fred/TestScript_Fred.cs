using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCC.Math.Graph;
using Questing;
using System;

public class TestScript_Fred : MonoBehaviour
{
    public int questBuilderIndex = 0;
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
            Debug.Log("quest added");
        }
    }
}