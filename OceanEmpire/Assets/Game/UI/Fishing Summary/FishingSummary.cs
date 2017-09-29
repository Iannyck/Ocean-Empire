using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSummary : MonoBehaviour
{
    public const string SCENENAME = "FishingSummary";

    private void Start()
    {
        CCC.Manager.MasterManager.Sync();
    }

    public void ShowReport(FishingReport report)
    {
        //A remplir
    }

    public void GoBackToShack()
    {
        LoadingScreen.TransitionTo(ShackManager.SCENENAME, null);
    }
}
