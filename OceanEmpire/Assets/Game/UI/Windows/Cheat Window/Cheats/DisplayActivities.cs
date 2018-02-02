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
            List<GoogleActivities.ActivityReport> records = GoogleActivities.instance.records;
            for (int i = 0; i < records.Count; i++)
            {
                allActivities += records[i].best.rate;
                allActivities += "|";
                allActivities += GoogleActivities.instance.priority.ExerciseTypeToString(records[i].best.type);
                allActivities += "->";
                allActivities += records[i].time;
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
