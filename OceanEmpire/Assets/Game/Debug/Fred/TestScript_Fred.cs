using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript_Fred : MonoBehaviour
{
    public DayInspector g;

    void Start()
    {
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");


        InstantExerciseChoice.ProposeTasks();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
        }
    }
}
