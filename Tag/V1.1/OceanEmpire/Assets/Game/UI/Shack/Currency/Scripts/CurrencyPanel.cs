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
        PersistentLoader.LoadIfNotLoaded(Listen);
    }

    void Listen()
    {
        UpdateCurrencyValues();
        Canvas.ForceUpdateCanvases();
        PlayerCurrency.TicketChange += AnimateTicketGain;
        PlayerCurrency.CoinChange += AnimateCoinGain;
        PlayerCurrency.CurrencyUpdate += UpdateCurrencyValues;

        if (PlayerCurrency.GetUnseenDeltaTickets() != 0)
        {
            AnimateTicketGain(PlayerCurrency.GetUnseenDeltaTickets());
            PlayerCurrency.SeeDeltaTickets();
        }
        if (PlayerCurrency.GetUnseenDeltaCoins() != 0)
        {
            AnimateTicketGain(PlayerCurrency.GetUnseenDeltaCoins());
            PlayerCurrency.SeeDeltaCoins();
        }
    }

    void AnimateTicketGain(int delta, CurrencyEventArgs e)
    {
        AnimateTicketGain(delta);
        e.Seen = true;
    }
    void AnimateTicketGain(int delta)
    {
        AnimateGain(delta, ticketChangePopup, ticketAmount);
    }

    void AnimateCoinGain(int delta, CurrencyEventArgs e)
    {
        AnimateCoinGain(delta);
        e.Seen = true;
    }
    void AnimateCoinGain(int delta)
    {
        AnimateGain(delta, coinChangePopup, moneyAmount);
    }

    void AnimateGain(int delta, TextPopup prefab, Text anchorText)
    {
        TextPopup textPopup = prefab.DuplicateGO(transform);
        textPopup.transform.position = anchorText.transform.position;
        textPopup.GetTextComponent().text = (delta > 0 ? "+" : "") + delta;
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
