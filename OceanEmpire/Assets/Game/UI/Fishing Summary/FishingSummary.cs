using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingSummary : MonoBehaviour
{
    public const string SCENENAME = "FishingSummary";

    private FishingReport fishingReport;

    public Text total;
    public string baseText;
    public GameObject fishSummaryPrefab;
    public Transform countainer;

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
            Instantiate(fishSummaryPrefab, countainer).GetComponent<FishSummary>().SetFishSummary( entry.Value, 
                                                                                    entry.Key.icon.GetSprite(),
                                                                                    entry.Key.fishName);
            fishes += entry.Value;
        }
        total.text = baseText + fishes;

        UpdateFishPopulation();
    }

    public void GoBackToShack()
    {
        LoadingScreen.TransitionTo(ShackManager.SCENENAME, new ToShackMessage(fishingReport));
    }

    public void UpdateFishPopulation()
    {
        float CapturedValue = 0;
        foreach (KeyValuePair<FishDescription, int> entry in fishingReport.CapturedFish)
        {
            CapturedValue += entry.Value * entry.Key.populationValue;
        }

        FishPopulation.instance.UpdateOnFishing(CapturedValue);
    }

}
