using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PurchaseReport
{
    public CurrencyAmount cost;
    public string itemName;
    public string itemID;
    public DateTime purchasedOn;

    public PurchaseReport(CurrencyAmount cost, string itemName, string itemID)
    {
        this.cost = cost;
        this.itemID = itemID;
        this.itemName = itemName;
        purchasedOn = DateTime.Now;
    }

    public override string ToString()
    {
        return "\nItem Name: " + itemName
            + "\nCost: " + cost.ToString()
            + "\nItem ID: " + itemID
            + "\nPurchased on: " + purchasedOn.ToString();
    }
}
