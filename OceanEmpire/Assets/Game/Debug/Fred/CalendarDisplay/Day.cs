using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CalendarUI
{
    public class Day : MonoBehaviour
    {
        public Text dayNumber;

        private Calendar.Day data;
        public void DisplayDay(Calendar.Day day)
        {
            data = day;
            dayNumber.text = day.dayOfMonth.ToString();
        }
    }
}