using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ItemDisplay : MonoBehaviour {

    public UnityAction OnEquip;
    public UnityAction OnBuyWithMoney;
    public UnityAction OnBuyWithTicket;

    public ShopButton moneyCostButton;
    public ShopButton fullButton;
    public ShopButton ticketCostButton;


    public ItemDescription item;

    public Text itemName;
    public Text itemDescription;

    public Sprite ticketImage;
    public Sprite moneyImage;

    public Image itemImage;

    // Use this for initialization
    void Start()
    {

        itemName.text = item.GetName();
        itemDescription.text = item.GetDescription();

        itemImage.sprite = item.GetImage();

        OnEquip += Equip;
        OnBuyWithMoney += BuyWithMoney;
        OnBuyWithTicket += BuyWithTicket;

        UpdateButton();
    }

    public void UpdateButton()
    {
        string itemID = item.GetItemID();

        if (ItemsList.ItemOwned(itemID))
        {
            if (ItemsList.instance.IsEquiped(itemID))
                ButtonEquiped();
            else
                ButtonOwned();
        }
        else {
            int moneyCost = item.GetMoneyCost();
            int ticketCost = item.GetTicketCost();

            if (moneyCost >= 0 && ticketCost >= 0)
                ButtonTicketAndMoneyCost();

            else
            {
                if (moneyCost <= 0 && ticketCost < 0)
                    ButtonMoneyCostOnly();
                else if (moneyCost > 0 && ticketCost <= 0)
                    ButtonTickeyCostOnly();
            }
        }

    }

    public void ButtonEquiped()
    {
        moneyCostButton.DisableButton();
        ticketCostButton.DisableButton();
        fullButton.EquipedButton();
    }

    public void ButtonOwned()
    {
        moneyCostButton.DisableButton();
        ticketCostButton.DisableButton();
        fullButton.OwnedButton(OnEquip);
    }

    public void ButtonTicketAndMoneyCost()
    {
        moneyCostButton.MoneyButton(OnBuyWithMoney, item.GetMoneyCost());
        ticketCostButton.TicketButton(OnBuyWithTicket, item.GetTicketCost());
        fullButton.DisableButton();
    }

    public void ButtonMoneyCostOnly()
    {
        moneyCostButton.DisableButton();
        ticketCostButton.DisableButton();
        fullButton.MoneyButton(OnBuyWithMoney, item.GetMoneyCost());
    }

    public void ButtonTickeyCostOnly()
    {
        moneyCostButton.DisableButton();
        ticketCostButton.DisableButton();
        fullButton.TicketButton(OnBuyWithTicket, item.GetTicketCost());
    }

    public void BuyWithMoney()
    {
        if (PlayerCurrency.GetCoins() > item.GetMoneyCost())
        {
            PlayerCurrency.RemoveCoins(item.GetMoneyCost());
            Buy();
        }
    }

    public void BuyWithTicket()
    {
        if (PlayerCurrency.GetTickets() > item.GetTicketCost())
        {
            PlayerCurrency.RemoveTickets(item.GetTicketCost());
            Buy();
        }
    }

    public void Buy()
    {
        if (item is UpgradeDescription)
        {
            ItemsList.BuyUpgrade(item.GetItemID());
            Equip();
        }
        if (item is ShopMapDescription)
        {
            ItemsList.BuyMap(item.GetItemID());
        }
        UpdateShop();
    }


    public void Equip()
    {
        ItemsList.EquipUpgrade(item as UpgradeDescription);
        UpdateShop();
    }



    public void UpdateShop()
    {
        GetComponentInParent<ShopUI>().UpdateDisplay();
    }

}
