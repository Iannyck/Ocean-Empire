using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingSummary : MonoBehaviour
{
    public const string SCENENAME = "FishingSummary";

    private FishingReport fishingReport;

    public float delayToShowPopulationChanges = 1f;

    public GameObject fishSummaryPrefab;
    public Transform countainer;

    public WidgetFishPop widgetFishPop;

    private void Start()
    {
        PersistentLoader.LoadIfNotLoaded();
        Time.timeScale = 1;
    }

    public void ShowReport(FishingReport report)
    {
        fishingReport = report;

        int  fishes = 0;
        foreach (KeyValuePair<FishDescription, int> entry in report.CapturedFish)
        {
            Instantiate(fishSummaryPrefab, countainer).GetComponent<FishSummary>().SetFishSummary(entry.Value, 
                                                                                    entry.Key.icon.GetSprite(),
                                                                                    (entry.Key.baseMonetaryValue * entry.Value).ToString());

            PlayerCurrency.AddCoins(entry.Value * (int)entry.Key.baseMonetaryValue);

            fishes += entry.Value;
        }

        this.DelayedCall(UpdateFishPopulation, delayToShowPopulationChanges);
        
    }

    public void UpdateFishPopulation()
    {
        float CapturedValue = 0;
        foreach (KeyValuePair<FishDescription, int> entry in fishingReport.CapturedFish)
        {
            CapturedValue += entry.Value;// * entry.Key.populationValue;
        }
        
        if (widgetFishPop != null)
        {
            float capturedRate = FishPopulation.instance.FishNumberToRate(CapturedValue);
            widgetFishPop.DecrementRate(capturedRate);
        }
        else
            FishPopulation.instance.UpdateOnFishing(CapturedValue);
    }

    public void GoBackToShack()
    {
        LoadingScreen.TransitionTo(Shack.SCENENAME, new ToShackMessage(fishingReport));
    }
}
