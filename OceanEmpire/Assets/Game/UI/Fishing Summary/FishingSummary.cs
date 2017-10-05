using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingSummary : MonoBehaviour
{
    public Text test;
    public const string SCENENAME = "FishingSummary";
    private FishingReport fishingReport;

    private void Start()
    {
        CCC.Manager.MasterManager.Sync();
    }

    public void ShowReport(FishingReport report)
    {
        fishingReport = report;
        int  fishes = 0;
        foreach (KeyValuePair<FishDescription, int> entry in report.CapturedFish)
        {
            fishes += entry.Value;
        }
        test.text = "Wow, good job.\n" + fishes + "\npoissons capturés! \n\nUn vrai champion!";

    }

    public void GoBackToShack()
    {
        LoadingScreen.TransitionTo(ShackManager.SCENENAME, new ToShackMessage());
    }
}
