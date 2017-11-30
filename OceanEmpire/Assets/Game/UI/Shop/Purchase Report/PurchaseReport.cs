using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PurchaseReport
{
    public Montant cost;
    public string itemName;
    public string itemID;
    public DateTime purchasedOn;
}
