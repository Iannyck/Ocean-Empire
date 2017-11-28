using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingSummary : MonoBehaviour
{
    public const string SCENENAME = "FishingSummary";

    private FishingReport fishingReport;

    public GameObject fishSummaryPrefab;
    public Transform fishSummaryContainer;

    public WidgetFishPop widgetFishPop;
    public SummaryCurrencyDisplay widgetCurrency;

    public float delayBeforeStart = 1.5f;


    private void Start()
    {
        CCC.Manager.MasterManager.Sync();
        Time.timeScale = 1;
    }

    public void ShowReport(FishingReport report)
    {
        fishingReport = report;

        float populationValue = 0;
        int monetaryValue = 0;

        foreach (KeyValuePair<FishDescription, int> entry in report.CapturedFish)
        {
            FishSummary newSummary = Instantiate(fishSummaryPrefab, fishSummaryContainer).GetComponent<FishSummary>();
            newSummary.SetFishSummary(entry.Key, entry.Value);

            populationValue += entry.Key.populationValue * entry.Value;
            monetaryValue += (entry.Key.baseMonetaryValue * entry.Value).RoundedToInt();
        }

        CCC.Manager.DelayManager.LocalCallTo( () =>
        {
            updateCurrency(monetaryValue);
            updatePopulation(populationValue);

        }, delayBeforeStart, this);
    }


    public void GoBackToShack()
    {
        LoadingScreen.TransitionTo(Shack.SCENENAME, new ToShackMessage(fishingReport));
    }



    private void updatePopulation(float populationValue)
    {
        if (widgetFishPop != null)
        {
            float capturedRate = FishPopulation.instance.FishNumberToRate(populationValue);
            widgetFishPop.DecrementRate(capturedRate);
        }
        else
            FishPopulation.instance.UpdateOnFishing(populationValue);
    }

    private void updateCurrency(int monetaryValue)
    {
        if (widgetCurrency != null)
            widgetCurrency.IncrementValues(monetaryValue, 0);
        PlayerCurrency.AddCoins(monetaryValue);
    }


    public void testIncrementMoney()
    {
        widgetCurrency.IncrementValues(500, 10);
    }

}
