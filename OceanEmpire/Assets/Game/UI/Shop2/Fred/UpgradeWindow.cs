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
    private UpgradeDescription nextDesc;

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
        Logger.Log(Logger.Category.Shop, 
            "Purchase: " + upgradeCategory.name
            + " lvl" + nextDesc.GetUpgradeLevel()
            + " cost(" + nextDesc.GetCost(CurrencyType.Ticket) + "tkt)");

        buyCallback();
        CloseWindow();
    }

    private void BuyWithCoins()
    {
        if (!upgradeCategory.Buy(CurrencyType.Coin))
        {
            MessagePopup.DisplayMessage("Pas assez de pièces bleus");
        }
        Logger.Log(Logger.Category.Shop,
            "Purchase: " + upgradeCategory.name
            + " lvl" + nextDesc.GetUpgradeLevel()
            + " cost(" + nextDesc.GetCost(CurrencyType.Coin) + "coin)");
        buyCallback();
        CloseWindow();
    }

    public void FillContent(UpgradeCategory upgradeCategory, Sprite icon, Action buyCallback)
    {
        this.upgradeCategory = upgradeCategory;
        this.buyCallback = buyCallback;

        var current = upgradeCategory.GetCurrentUpgradeDescription();
        var next = nextDesc = upgradeCategory.GetNextUpgradeDescription();

        var costCoin = next.GetCost(CurrencyType.Coin);
        var costTicket = next.GetCost(CurrencyType.Ticket);

        string buyPrefix = current == null ? "Débloquer: " : "Améliorer: ";

        buyWithCoinsButton.GetComponentInChildren<Text>().text = buyPrefix + costCoin;
        buyWithCoinsButton.interactable = costCoin <= PlayerCurrency.GetCoins();
        buyWithCoinsButton.gameObject.SetActive(costCoin != -1);

        buyWithTicketButton.GetComponentInChildren<Text>().text = buyPrefix + costTicket;
        buyWithTicketButton.interactable = costTicket <= PlayerCurrency.GetTickets();
        buyWithTicketButton.gameObject.SetActive(costTicket != -1);

        var nextStats = next.GetStatistics();
        bool[] colored = new bool[nextStats.Count];

        if (current != null)
        {
            var currentStats = current.GetStatistics();

            // List of colored stats
            for (int i = 0; i < nextStats.Count && i < currentStats.Count; i++)
            {
                colored[i] = nextStats[i].value != currentStats[i].value;
            }
            currentUIItem.FillStats(currentStats, colored, negativeColor);


            currentUIItem.FillTitle(upgradeCategory.CategoryName);
            currentUIItem.FillCurrentContent(current);
            currentUIItem.FillIcon(icon);
        }
        else
        {
            currentUIItem.gameObject.SetActive(false);
        }

        nextUIItem.FillTitle(upgradeCategory.CategoryName);
        nextUIItem.FillCurrentContent(next);
        nextUIItem.FillIcon(icon);
        nextUIItem.FillStats(nextStats, colored, positiveColor);
    }

    void CloseWindow()
    {
        Scenes.UnloadAsync(gameObject.scene);
    }
}
