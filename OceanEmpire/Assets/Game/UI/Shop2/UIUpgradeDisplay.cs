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
    public TierColors tierColors;

    [Header("UI")]
    public Text levelText;
    public Text statsText;
    public Text ticketCostText;
    public Text coinCostText;
    public Image image;
    public Image coloredBG;
    public GameObject canUpgradeContent;
    public GameObject cannotUpgradeContent;
    public Text titleText;

    void Start()
    {
        UpdateDefaultContent();
    }

    public void UpdateDefaultContent()
    {
        if (category != null)
        {
            FillTitle(category.CategoryName);
            FillCurrentContent(category.GetCurrentUpgradeDescription());
            FillNextContent(category.GetNextUpgradeDescription());
        }
    }

    public void FillTitle(string title)
    {
        if (titleText != null)
            titleText.text = title;
    }
    public void FillStats(string stats, string statTextColor)
    {
        if (statsText != null)
        {
            stats = stats.Replace("<stat>", "<color=" + statTextColor + '>');
            stats = stats.Replace("</stat>", "</color>");
            statsText.text = stats;
        }
    }
    public void FillStats(List<Statistic> statistics, List<bool> colored, string coloredTextColor)
    {
        if (statsText != null)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < statistics.Count; i++)
            {
                str.Append(statistics[i].name);
                str.Append(": ");
                if (colored[i])
                {
                    str.Append("<color=");
                    str.Append(coloredTextColor);
                    str.Append('>');
                }


                if (statistics[i].suffix == "s" && statistics[i].value >= 60)
                {
                    int minutes = Mathf.FloorToInt(statistics[i].value / 60f);
                    int seconds = (int)statistics[i].value % 60;
                    str.Append(minutes);
                    str.Append("min ");
                    if (seconds != 0)
                    {
                        str.Append(seconds);
                        str.Append('s');
                    }
                }
                else
                {
                    str.Append(statistics[i].value);
                    str.Append(statistics[i].suffix);
                }


                if (colored[i])
                {
                    str.Append("</color>");
                }
                if (i < statistics.Count - 1)
                    str.Append('\n');
            }

            statsText.text = str.ToString();
        }
    }
    public void FillIcon(Sprite icon)
    {
        if (image != null && icon != null)
            image.sprite = icon;
    }

    public void FillCurrentContent(UpgradeDescription currentUpgrade)
    {
        if (currentUpgrade != null)
        {
            if (coloredBG != null)
                coloredBG.color = tierColors.colors[currentUpgrade.GetUpgradeLevel() - 1];
            if (levelText != null)
                levelText.text = "Niv " + (currentUpgrade.GetUpgradeLevel());
        }
        else
        {
            if (levelText != null)
                levelText.text = "Niv -";
            if (statsText != null)
                statsText.text = "--";
        }
    }

    public void FillNextContent(UpgradeDescription nextUpgrade)
    {
        if (nextUpgrade != null)
        {
            if (coinCostText != null)
                coinCostText.text = PriceToBeautifulString(nextUpgrade.GetCost(CurrencyType.Coin));
            if (ticketCostText != null)
                ticketCostText.text = PriceToBeautifulString(nextUpgrade.GetCost(CurrencyType.Ticket));

            if (canUpgradeContent != null)
                canUpgradeContent.SetActive(true);
            if (cannotUpgradeContent != null)
                cannotUpgradeContent.SetActive(false);
        }
        else
        {
            if (coinCostText != null)
                coinCostText.text = "--";
            if (ticketCostText != null)
                ticketCostText.text = "--";

            if (canUpgradeContent != null)
                canUpgradeContent.SetActive(false);
            if (cannotUpgradeContent != null)
                cannotUpgradeContent.SetActive(true);
        }
    }

    public static string PriceToBeautifulString(int value)
    {
        string txt = value.ToString();
        if (txt.Length > 3)
            txt = txt.Insert(txt.Length - 3, "'");
        return txt;
    }
}
