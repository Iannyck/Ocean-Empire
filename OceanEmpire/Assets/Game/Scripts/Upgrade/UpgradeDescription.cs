using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UpgradeDescription
{
    [SerializeField]
    private string itemName;
    [SerializeField]
    private string itemDescription;

    public int GetCost(CurrencyType type)
    {
        switch (type)
        {
            case CurrencyType.Coin:
                return moneyCost;

            case CurrencyType.Ticket:
                return ticketCost;

            default:
                return 2147483647;
        }
    }

    [SerializeField]
    private int moneyCost;
    [SerializeField]
    private int ticketCost;

    [SerializeField]
    private Sprite itemIcon;


    public Sprite GetShopIcon()
    {
        return itemIcon;
    }
    public string GetTitle()
    {
        return itemName;
    }
    public string GetDescription()
    {
        return itemDescription;
    }
}
