using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarScroll_Schedule : MonoBehaviour
{
    [SerializeField] private Text textComponent;

    public void FillContent(TimeSlot timeSlot, string label)
    {
        textComponent.text = timeSlot.ToCondensedDayOfTimeString() + "   " + label;
    }
}
