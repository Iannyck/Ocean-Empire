using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript_Fred : MonoBehaviour
{
    public Calendar calendar;

    void Start()
    {
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            calendar.AddBonifiedTime(new BonifiedTime(new TimeSlot(DateTime.Now, new TimeSpan(0, 0, 20)), 2));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            calendar.AddBonifiedTime(new BonifiedTime(new TimeSlot(DateTime.Now.AddDays(2), new TimeSpan(0, 0, 20)), 2));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            calendar.AddBonifiedTime(new BonifiedTime(new TimeSlot(DateTime.Now.AddDays(10), new TimeSpan(0, 0, 20)), 2));
        }
    }
}