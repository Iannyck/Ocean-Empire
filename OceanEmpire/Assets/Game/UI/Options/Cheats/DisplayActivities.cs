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
            int iterationMax = 50;
            string allActivities = "";
            List<ActivityDetection.Activity> activities = ActivityAnalyser.instance.activities;
            for (int i = 0; i < activities.Count; i++)
            {
                if (i > iterationMax)
                    break;
                allActivities += activities[i].probability;
                allActivities += "->";
                allActivities += activities[i].time;
                allActivities += "|";
                if(i+1 < activities.Count)
                {
                    allActivities += activities[i + 1].probability;
                    allActivities += "->";
                    allActivities += activities[i + 1].time;
                    allActivities += "|";
                    allActivities += "\n";
                    i++; // oui, c'est voulu
                }
            }
            display.text = allActivities;
        });
    }

    public void Close()
    {
        anim.Close();
    }
}
