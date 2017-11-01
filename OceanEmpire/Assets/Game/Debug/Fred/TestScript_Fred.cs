using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript_Fred : MonoBehaviour
{
    public CalendarDisplay display;
    public Calendar calendar = new Calendar();

    void Start()
    {
        display.DisplayCalendar(calendar);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            display.UpdateDisplay();
        }
    }
}
