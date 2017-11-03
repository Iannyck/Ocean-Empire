using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript_Fred : MonoBehaviour
{
    public CalendarScroll_Controller scroller;
    public Calendar calendar = new Calendar();

    void Start()
    {
        scroller.Fill(calendar);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            scroller.Fill(calendar);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            scroller.GoBackwardOneWeek();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            scroller.GoForwardOneWeek();
        }
    }
}
