using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyPanel : MonoBehaviour {

    public Text moneyAmount;
    public Text tickeyAmount;

    private bool ready = false;

    // Use this for initialization
    void Start () {
        Init();
    }

    private void Init()
    {
        if (PlayerCurrency.instance != null)
        {
            UpdateCurrencyValues();
            PlayerCurrency.CurrencyUpdate += UpdateCurrencyValues;
            ready = true;
        }
    }

    void Update()
    {
        if (ready == false)
            Init();
    }


    void UpdateCurrencyValues ()
    {
        moneyAmount.text = PlayerCurrency.GetCoins().ToString();
        tickeyAmount.text = PlayerCurrency.GetTickets().ToString();
    }

    void OnDestroy()
    {
        PlayerCurrency.CurrencyUpdate -= UpdateCurrencyValues;
    }


}
