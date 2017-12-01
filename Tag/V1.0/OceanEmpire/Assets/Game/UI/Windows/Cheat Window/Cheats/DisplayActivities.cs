using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCC.UI;

public class DisplayActivities : MonoBehaviour {

    public WindowAnimation anim;
    public Text display;

	public void Display()
    {
        anim.Open(delegate ()
        {
            MessagePopup.DisplayMessage("Showing The Activity File");
            string allActivities = "";
            List<ActivityDetection.Activity> activities = ActivityAnalyser.instance.activities;
            for (int i = 0; i < activities.Count; i++)
            {
                allActivities += activities[i].probability;
                allActivities += "->";
                allActivities += activities[i].time;
                allActivities += "\n";
            }
            display.text = allActivities;
        });
    }

    public void Close()
    {
        anim.Close();
    }
}
