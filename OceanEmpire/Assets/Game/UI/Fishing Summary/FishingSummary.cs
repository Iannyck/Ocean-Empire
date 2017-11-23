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
    public WidgetFishPop widgetFishPop;

    private void Start()
    {
        CCC.Manager.MasterManager.Sync();
        Time.timeScale = 1;
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

            PlayerCurrency.AddCoins(entry.Value * (int)entry.Key.baseMonetaryValue);

            fishes += entry.Value;
        }
        total.text = baseText + fishes;



        CCC.Manager.DelayManager.LocalCallTo(delegate () { UpdateFishPopulation();  }, 1f , this);
        
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

       
        if (widgetFishPop != null)
        {
            float capturedRate = FishPopulation.instance.FishNumberToRate(CapturedValue);
            widgetFishPop.DecrementRate(capturedRate);
        }
        else
            FishPopulation.instance.UpdateOnFishing(CapturedValue);
    }
}
