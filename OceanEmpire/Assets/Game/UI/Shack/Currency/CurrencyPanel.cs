using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCC.Manager;

public class CurrencyPanel : MonoBehaviour
{
    public Text moneyAmount;
    public Text tickeyAmount;

    private bool ready = false;

    void OnEnable()
    {
        if (PlayerCurrency.instance == null)
            MasterManager.Sync(Listen);
        else
            Listen();
    }

    void Listen()
    {
        UpdateCurrencyValues();
        PlayerCurrency.CurrencyUpdate += UpdateCurrencyValues;
    }


    void UpdateCurrencyValues()
    {
        moneyAmount.text = PlayerCurrency.GetCoins().ToString();
        tickeyAmount.text = PlayerCurrency.GetTickets().ToString();
    }

    void OnDisable()
    {
        if (PlayerCurrency.instance != null)
            PlayerCurrency.CurrencyUpdate -= UpdateCurrencyValues;
    }
}
