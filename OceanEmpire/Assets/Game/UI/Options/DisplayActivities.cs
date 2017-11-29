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
            string allActivities = "";
            foreach (var activity in ActivityAnalyser.instance.activites)
            {
                allActivities += activity.probability;
                allActivities += "->";
                allActivities += activity.time;
                allActivities += "|";
            }
            display.text = allActivities;
        });
    }

    public void Close()
    {
        anim.Close();
    }
}
