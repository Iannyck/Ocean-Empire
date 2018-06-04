using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemDisplay2 : MonoBehaviour {


    private IShopDisplayable displayableCategory;

    public Text titre;
    public Image icon;

    public Text statsNames;
    public Text statsValues;

    public ShopButton2 coinCostButton;
    public ShopButton2 ticketCostButton;

    public void SetValues(IShopDisplayable category)
    {
        displayableCategory = category;
        UpdateView();
    }

    public void UpdateView()
    {

        titre.text = displayableCategory.GetTitle();
        string zone1 = "";
        string zone2 = "";

        if (displayableCategory is IUpgradeDisplayable)
        {
            List<Statistic> stats = ((IUpgradeDisplayable)displayableCategory).GetStatistics();
            for (int i = 0; i < stats.Count; i++)
            {
                zone1 = zone1 + stats[i].name;
                zone2 = zone2 + stats[i].value.ToString("F");
                if(i < stats.Count - 1)
                {
                    zone1 = zone1 + "\n";
                    zone2 = zone2 + "\n";
                }
            }
            statsNames.text = zone1;
            statsValues.text = zone2;
             
        }

        icon.sprite = displayableCategory.GetShopIcon();

        coinCostButton.SetButton(Buy, displayableCategory.GetPrice(CurrencyType.Coin), CurrencyType.Coin);
        ticketCostButton.SetButton(Buy, displayableCategory.GetPrice(CurrencyType.Ticket), CurrencyType.Ticket);
    }

    public void Buy(CurrencyType type)
    {
        if (PlayerCurrency.GetCurrency(type) > displayableCategory.GetPrice(type))
        {
            ConfirmBuy.OpenWindowAndConfirm(type, (hasConfirmed) =>
            {
                if (hasConfirmed)
                {
                    displayableCategory.Buy(type);
                    Scenes.GetActive(ItemDescScene.SCENENAME).FindRootObject<ItemDescScene>().Quit();
                }
            });
        }
    }
}
