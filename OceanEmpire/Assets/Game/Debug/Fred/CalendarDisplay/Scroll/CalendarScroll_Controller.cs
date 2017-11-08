﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarScroll_Controller : MonoBehaviour
{
    [Header("Links")]
    public CalendarScroll_Scroller scroller;

    [Header("Settings")]
    public int startingDayIndex = 1;

    void Start()
    {
        Refill();
    }

    private void Refill()
    {
        scroller.Fill(Calendar.GetDaysFrom(DateTime.Now.AddDays(-startingDayIndex), scroller.days.Count));
    }

    public void BackToTop()
    {
        Refill();
    }

    public void BackToBottom()
    {
        Refill();
    }
}
