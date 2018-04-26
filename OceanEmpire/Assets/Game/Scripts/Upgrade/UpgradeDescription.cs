using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class UpgradeDescription
{
    public UpgradeDescription() { }
    public UpgradeDescription(string nm, int lv, string desc, int coin, int tick, Sprite Icon)
    {
        itemName = nm;
        upgradeLevel = lv;
        itemDescription = desc;
        moneyCost = coin;
        ticketCost = tick;
        itemIcon = Icon;
    }

    [SerializeField]
    private string itemName;
    [SerializeField]
    private int upgradeLevel;
    [SerializeField, TextArea]
    private string itemDescription;
    [SerializeField]
    private int moneyCost;
    [SerializeField]
    private int ticketCost;
    [SerializeField]
    private Sprite itemIcon;

    public ISpriteKit spriteKit;

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

    public Sprite GetShopIcon()
    {
        return itemIcon;
    }
    public int GetUpgradeLevel()
    {
        return upgradeLevel;
    }
    public string GetTitle()
    {
        return itemName;
    }
    public string GetDescription()
    {
        return itemDescription;
    }

    abstract public List<Statistic> GetStatistics();
}
