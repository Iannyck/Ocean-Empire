using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 

public class CurrencyPanel : MonoBehaviour
{
    [SerializeField] private Text moneyAmount;
    [SerializeField] private Text ticketAmount;

    [Header("Popups"), SerializeField] private TextPopup coinChangePopup;
    [SerializeField] private TextPopup ticketChangePopup;

    void OnEnable()
    {
        if (PlayerCurrency.instance == null)
            PersistentLoader.LoadIfNotLoaded(Listen);
        else
            Listen();
    }

    void Listen()
    {
        UpdateCurrencyValues();
        PlayerCurrency.TicketChange += AnimateTicketGain;
        PlayerCurrency.CoinChange += AnimateCoinGain;
        PlayerCurrency.CurrencyUpdate += UpdateCurrencyValues;
    }

    void AnimateTicketGain(int delta)
    {
        print((delta > 0 ? "+" : "") + delta + " tickets");
        TextPopup textPopup = ticketChangePopup.DuplicateGO(transform);
        textPopup.transform.position = ticketAmount.transform.position;
        textPopup.GetTextComponent().text = (delta > 0 ? "+" : "") + delta;
    }

    void AnimateCoinGain(int delta)
    {
        print((delta > 0 ? "+" : "") + delta + " coins");
    }

    void UpdateCurrencyValues()
    {
        moneyAmount.text = PlayerCurrency.GetCoins().ToString();
        ticketAmount.text = PlayerCurrency.GetTickets().ToString();
    }

    void OnDisable()
    {
        if (PlayerCurrency.instance != null)
            PlayerCurrency.TicketChange -= AnimateTicketGain;
        if (PlayerCurrency.instance != null)
            PlayerCurrency.CoinChange -= AnimateCoinGain;
        if (PlayerCurrency.instance != null)
            PlayerCurrency.CurrencyUpdate -= UpdateCurrencyValues;
    }
}
