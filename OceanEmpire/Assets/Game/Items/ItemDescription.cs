using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu(menuName = "Ocean Empire/Item Description")]
public class ItemDescription : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int moneyCost;
    public int ticketCost;
    public Sprite itemImage;

    public string GetName()
    {
        return itemName;
    }

    public string GetDescription()
    {
        return itemDescription;
    }

    public int GetMoneyCost()
    {
        return moneyCost;
    }

    public int GetTicketCost()
    {
        return ticketCost;
    }

    public Sprite GetImage()
    {
        return itemImage;
    }
}
