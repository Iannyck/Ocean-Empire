using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class UIUpgradeDisplay : MonoBehaviour
{
    [Header("Data")]
    public UpgradeCategory category;

    [Header("UI")]
    public Text levelText;
    public Text ticketCostText;
    public Text coinCostText;

    void Start()
    {
        if (category != null)
        {
            FillCurrentContent(category.GetCurrentUpgradeDescription());
            FillNextContent(category.GetNextUpgradeDescription());
        }
    }

    public void FillCurrentContent(UpgradeDescription currentUpgrade)
    {
        if (currentUpgrade != null)
        {
            if (levelText != null)
                levelText.text = "Niv " + currentUpgrade.GetUpgradeLevel();
        }
        else
        {
            if (levelText != null)
                levelText.text = "Niv -";
        }
    }

    public void FillNextContent(UpgradeDescription nextUpgrade)
    {
        if (nextUpgrade != null)
        {
            if (coinCostText != null)
                coinCostText.text = nextUpgrade.GetCost(CurrencyType.Coin).ToString();
            if (ticketCostText != null)
                ticketCostText.text = nextUpgrade.GetCost(CurrencyType.Ticket).ToString();
        }
        else
        {
            if (coinCostText != null)
                coinCostText.text = "--";
            if (ticketCostText != null)
                ticketCostText.text = "--";
        }
    }
}
