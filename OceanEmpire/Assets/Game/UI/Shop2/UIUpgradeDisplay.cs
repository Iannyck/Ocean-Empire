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

    [Header("Availability")]
    public GameObject availableVisuals;
    public GameObject notAvailableVisuals;

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
    public RectTransform costsPanel;

    [Header("Yet to be unlocked")]
    public Color ytbuBGColor;
    public Color ytbuImageColor;
    public Sprite ytbuTitlePanelSprite;
    public Sprite ytbuCostPanelLabelSprite;
    public Sprite ytbuCostPanelBGSprite;
    public Image ytbuTitlePanel;
    public Image ytbuCostPanelLabel;
    public Image ytbuCostPanelBG;
    public GameObject ytbuBuyTextToEnable;
    public GameObject ytbuBuyTextToDisable;
    //public GameObject ytbu

    private Sprite stdTitlePanelSprite;
    private Sprite stdCostPanelLabelSprite;
    private Sprite stdCostPanelBGSprite;

    void Awake()
    {
        if (ytbuTitlePanel)
            stdTitlePanelSprite = ytbuTitlePanel.sprite;
        if (ytbuCostPanelLabel)
            stdCostPanelLabelSprite = ytbuCostPanelLabel.sprite;
        if (ytbuCostPanelBG)
            stdCostPanelBGSprite = ytbuCostPanelBG.sprite;
    }

    void Start()
    {
        UpdateDefaultContent();
    }

    public void UpdateDefaultContent()
    {
        if (category != null)
        {
            var available = category.IsAvailable;

            availableVisuals.SetActive(available);
            notAvailableVisuals.SetActive(!available);

            if (availableVisuals.activeSelf)
            {
                FillTitle(category.CategoryName);
                FillCurrentContent(category.GetCurrentUpgradeDescription());
                FillNextContent(category.GetNextUpgradeDescription());
            }
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
    public void FillStats(List<Statistic> statistics, bool[] colored, string coloredTextColor)
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

            if (levelText)
            {
                levelText.transform.parent.gameObject.SetActive(true);
                levelText.text = "Niv " + (currentUpgrade.GetUpgradeLevel());
            }

            if (ytbuCostPanelLabel)
                ytbuCostPanelLabel.sprite = stdCostPanelLabelSprite;
            if (ytbuCostPanelBG)
                ytbuCostPanelBG.sprite = stdCostPanelBGSprite;
            if (ytbuTitlePanel)
                ytbuTitlePanel.sprite = stdTitlePanelSprite;

            if (image)
                image.color = Color.white;


            if (ytbuBuyTextToEnable)
                ytbuBuyTextToEnable.SetActive(false);
            if (ytbuBuyTextToDisable)
                ytbuBuyTextToDisable.SetActive(true);
        }
        else
        {
            if (levelText != null)
                levelText.transform.parent.gameObject.SetActive(false);

            // YET TO BE UNLOCKED !!
            ytbuCostPanelLabel.sprite = ytbuCostPanelLabelSprite;
            ytbuCostPanelBG.sprite = ytbuCostPanelBGSprite;
            ytbuTitlePanel.sprite = ytbuTitlePanelSprite;
            coloredBG.color = ytbuBGColor;
            image.color = ytbuImageColor;

            ytbuBuyTextToEnable.SetActive(true);
            ytbuBuyTextToDisable.SetActive(false);
        }
    }

    public void FillNextContent(UpgradeDescription nextUpgrade)
    {
        if (nextUpgrade != null)
        {
            var coinCost = nextUpgrade.GetCost(CurrencyType.Coin);
            var ticketCost = nextUpgrade.GetCost(CurrencyType.Ticket);

            if (coinCost == -1 || ticketCost == -1)
                costsPanel.sizeDelta = new Vector2(costsPanel.sizeDelta.x, 92);
            else
                costsPanel.sizeDelta = new Vector2(costsPanel.sizeDelta.x, 146);

            if (coinCostText != null)
            {
                coinCostText.gameObject.SetActive(coinCost != -1);
                coinCostText.text = PriceToBeautifulString(coinCost);
            }
            if (ticketCostText != null)
            {
                ticketCostText.gameObject.SetActive(ticketCost != -1);
                ticketCostText.text = PriceToBeautifulString(ticketCost);
            }

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
