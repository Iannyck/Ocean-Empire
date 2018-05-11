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
    public Sprite bonusGoldSprite;

    //public WidgetFishPop widgetFishPop;

    private void Start()
    {
        PersistentLoader.LoadIfNotLoaded();
        Time.timeScale = 1;
    }

    public void ShowReport(FishingReport report)
    {
        fishingReport = report;

        int totalFish = 0;
        int totalCoins = 0;
        foreach (KeyValuePair<FishDescription, int> entry in report.CapturedFish)
        {
            Instantiate(fishSummaryPrefab, countainer).GetComponent<FishSummary>()
                .SetFishSummary(
                    entry.Value,
                    entry.Key.icon,
                    (entry.Key.baseMonetaryValue * entry.Value).ToString());

            totalCoins += entry.Value * (int)entry.Key.baseMonetaryValue;
            totalFish += entry.Value;
        }
        PlayerCurrency.AddCoins(totalCoins);

        Logger.Log(Logger.Category.GameEvent, 
            (report.wasFishingFrenzy ? "Super-peche" : "Peche") +
            ": fish(" + totalFish + ") coins(" + totalCoins + ")");

        if (report.harpoonBonusGold > 0)
            Instantiate(fishSummaryPrefab, countainer).GetComponent<FishSummary>()
                .SetFishSummary(
                    report.harpoonBonusCount,
                    bonusGoldSprite,
                    report.harpoonBonusGold.ToString());

    }

    public void GoBackToShack()
    {
        LoadingScreen.TransitionTo(Shack.SCENENAME, new ToShackMessage(fishingReport));
    }
}
