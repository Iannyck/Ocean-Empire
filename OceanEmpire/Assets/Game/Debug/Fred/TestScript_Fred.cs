using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript_Fred : MonoBehaviour
{
    public DayInspector g;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            InstantExerciseChoice.ProposeTasks();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
        }
    }
}
