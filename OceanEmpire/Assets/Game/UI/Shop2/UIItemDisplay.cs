using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public enum FishingConsumable{bait, harpoon}

public class UIItemDisplay : MonoBehaviour
{
    [Header("User config")]
    public FishingConsumable itemType;
    public Sprite itemSprite;
    [RangeAttribute(0,10)]
    public int tier = 0;
    public int amount;
    public int ticketCost;
    public int coinCost;

    [Header("Internal links")]
    public TierColors tierColors;
    public Text ownedText;
    public Text titleText;
    public Image image;
    public Text amountText;
    public Image coloredBG;
    public RectTransform costsPanel;
    public Text ticketCostText;
    public Text coinCostText;

    private Sprite stdTitlePanelSprite;
    private Sprite stdCostPanelLabelSprite;
    private Sprite stdCostPanelBGSprite;

    void Update()
    {
        ShowOwned();
    }

    void Start()
    {
        FillContent();
    }

    public void onClick()
    {
        switch(itemType)
        {
            case FishingConsumable.bait:
                if(PlayerCurrency.GetCoins() >= coinCost
                && PlayerCurrency.GetTickets() >= ticketCost
                && FishingFrenzy.Instance.State == FishingFrenzy.EffectState.InCooldown)
                {
                    PlayerCurrency.RemoveCoins(coinCost);
                    PlayerCurrency.RemoveTickets(ticketCost);

                    FishingFrenzy.Instance.Cheat_SkipCooldown();
                }
                break;
            case FishingConsumable.harpoon:
                if(PlayerCurrency.GetCoins() >= coinCost &&
                PlayerCurrency.GetTickets() >= ticketCost)
                {
                    PlayerCurrency.RemoveCoins(coinCost);
                    PlayerCurrency.RemoveTickets(ticketCost);

                    PlayerCurrency.AddHarpoons(amount);
                }
                break;
            default: break;
        }
        ShowOwned();
    }

    [ExecuteInEditMode][ContextMenu("MAJ visuelle")]
    public void FillContent()
    {
        ShowOwned();

        if (titleText != null)
        {
            switch(itemType)
            {
                case FishingConsumable.bait:
                    titleText.text = "Appâts";
                    break;
                case FishingConsumable.harpoon:
                    titleText.text = "Harpons";
                    break;
                default: break;
            }
        }

        if (image != null && itemSprite != null) image.sprite = itemSprite;

        if (amountText != null) amountText.text = "x " + amount.ToString();

        if (coloredBG != null) coloredBG.color = tierColors.colors[tier];

        if (costsPanel)
        {
            costsPanel.sizeDelta = new Vector2(costsPanel.sizeDelta.x, 150);

            if ((ticketCost <= 0) != (coinCost <= 0))
                costsPanel.sizeDelta = new Vector2(costsPanel.sizeDelta.x, 90);

            if ((ticketCost <= 0) && (coinCost <= 0))
                costsPanel.sizeDelta = new Vector2(costsPanel.sizeDelta.x, 0);
        }        

        if (ticketCostText != null)
        {
            if (ticketCost <= 0) ticketCostText.gameObject.SetActive(false);
            else ticketCostText.text = ticketCost.ToString();
        }
        
        if (coinCostText != null)
        {
            if (coinCost <= 0) coinCostText.gameObject.SetActive(false);
            else coinCostText.text = coinCost.ToString();
        }
    }

    public void ShowOwned()
    {
        if (ownedText != null && PlayerCurrency.instance != null)
        {
            switch(itemType)
            {
                case FishingConsumable.harpoon:
                    ownedText.text = PriceToBeautifulString(PlayerCurrency.GetHarpoons()) + " en stock";
                    break;
                case FishingConsumable.bait:
                    ownedText.text = FishingFrenzy.Instance.State == FishingFrenzy.EffectState.InCooldown ?
                        "Ne plus attendre !" :
                        "Déjà disponnible !";
                    break;
            }
        }
    }

    public string PriceToBeautifulString(int value)
    {
        string txt = value.ToString();
        if (txt.Length > 3)
            txt = txt.Insert(txt.Length - 3, "'");
        return txt;
    }
}