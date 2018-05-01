using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWindow : MonoBehaviour
{
    public UIUpgradeDisplay currentUIItem;
    public UIUpgradeDisplay nextUIItem;
    public string positiveColor;
    public string negativeColor;

    public Button buyWithCoinsButton;
    public Button buyWithTicketButton;
    public Button cancelButton;

    private UpgradeCategory upgradeCategory;
    private Action buyCallback;

    void Awake()
    {
        buyWithCoinsButton.onClick.AddListener(BuyWithCoins);
        buyWithTicketButton.onClick.AddListener(BuyWithTicket);
        cancelButton.onClick.AddListener(CloseWindow);
    }

    private void BuyWithTicket()
    {
        if (!upgradeCategory.Buy(CurrencyType.Ticket))
        {
            MessagePopup.DisplayMessage("Pas assez de tickets");
        }
        buyCallback();
        CloseWindow();
    }

    private void BuyWithCoins()
    {
        if (!upgradeCategory.Buy(CurrencyType.Coin))
        {
            MessagePopup.DisplayMessage("Pas assez de pièces bleus");
        }
        buyCallback();
        CloseWindow();
    }

    public void FillContent(UpgradeCategory upgradeCategory, Sprite icon, Action buyCallback)
    {
        this.upgradeCategory = upgradeCategory;
        this.buyCallback = buyCallback;

        var current = upgradeCategory.GetCurrentUpgradeDescription();
        var next = upgradeCategory.GetNextUpgradeDescription();

        buyWithCoinsButton.GetComponentInChildren<Text>().text = "Améliorer: " + next.GetCost(CurrencyType.Coin);
        buyWithTicketButton.GetComponentInChildren<Text>().text = "Améliorer: " + next.GetCost(CurrencyType.Ticket);

        buyWithCoinsButton.interactable = next.GetCost(CurrencyType.Coin) <= PlayerCurrency.GetCoins();
        buyWithTicketButton.interactable = next.GetCost(CurrencyType.Ticket) <= PlayerCurrency.GetTickets();

        var nextStats = next.GetStatistics();
        var currentStats = current.GetStatistics();

        // List of colored stats
        List<bool> colored = new List<bool>(nextStats.Count);

        for (int i = 0; i < nextStats.Count && i < currentStats.Count; i++)
        {
            colored.Add(nextStats[i].value != currentStats[i].value);
        }

        currentUIItem.FillTitle(upgradeCategory.CategoryName);
        nextUIItem.FillTitle(upgradeCategory.CategoryName);

        currentUIItem.FillCurrentContent(current);
        currentUIItem.FillIcon(icon);
        currentUIItem.FillStats(currentStats, colored, negativeColor);

        nextUIItem.FillCurrentContent(next);
        nextUIItem.FillIcon(icon);
        nextUIItem.FillStats(nextStats, colored, positiveColor);
    }

    void CloseWindow()
    {
        Scenes.UnloadAsync(gameObject.scene);
    }
}
