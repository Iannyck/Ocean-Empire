using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShopButton : MonoBehaviour {

    
    public Button button;
    public Text buttonText;
    public Image currencyIcon;

    private const string equipText = "Équipé";
    private const string ownedText = "Utiliser";

    void Start () {
        //gameObject.SetActive(false);
        if(button == null)
            button = gameObject.GetComponent<Button>();
    }

    public void EquipedButton()
    {
        gameObject.SetActive(true);
        currencyIcon.gameObject.SetActive(false);

        buttonText.text = equipText;
        button.interactable = false;

        button.onClick.RemoveAllListeners();   
    }

    public void OwnedButton(UnityAction OnEquip)   
    {
        gameObject.SetActive(true);
        currencyIcon.gameObject.SetActive(false);

        buttonText.text = ownedText;

        button.interactable = true;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnEquip);
    }
    
    public void MoneyButton(UnityAction OnBuyWithMoney, int moneyCost)
    {
        gameObject.SetActive(true);

        currencyIcon.gameObject.SetActive(true);
        currencyIcon.sprite = PlayerCurrency.GetMoneyIcon();

        buttonText.text = moneyCost.ToString();

        if (PlayerCurrency.GetCoins() > moneyCost)
            button.interactable = true;
        else
            button.interactable = false;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnBuyWithMoney);

    }

    public void TicketButton(UnityAction OnBuyWithTicket, int ticketCost)
    {
        gameObject.SetActive(true);

        currencyIcon.gameObject.SetActive(true);
        currencyIcon.sprite = PlayerCurrency.GetTicketIcon();

        buttonText.text = ticketCost.ToString();

        if (PlayerCurrency.GetTickets() > ticketCost)
            button.interactable = true;
        else
            button.interactable = false;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnBuyWithTicket);

    }

    public void DisableButton()
    {
        gameObject.SetActive(false);
    }
}
