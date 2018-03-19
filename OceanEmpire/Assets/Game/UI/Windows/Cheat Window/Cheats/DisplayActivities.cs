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
            MessagePopup.DisplayMessage("Showing The Activities");
            string allActivities = "";
            List<GoogleReader.Activity> activities = GoogleActivities.instance.activities;
            for (int i = 0; i < activities.Count; i++)
            {
                allActivities += activities[i].probabilities[0];
                allActivities += "|";
                allActivities += activities[i].probabilities[1];
                allActivities += "|";
                allActivities += activities[i].probabilities[2];
                allActivities += "|";
                allActivities += "W/R/B|";
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
