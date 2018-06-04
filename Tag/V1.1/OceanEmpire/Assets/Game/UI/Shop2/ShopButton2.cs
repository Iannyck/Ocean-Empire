using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class ShopButton2 : MonoBehaviour
{


    public Button button;
    public Text buttonText;
    public Image currencyIcon;

    private const string equipText = "Équipé";
    private const string ownedText = "Utiliser";

    void Start()
    {

        if (button == null)
            button = gameObject.GetComponent<Button>();
    }


    public void SetButton(Action<CurrencyType> OnBuy, int Cost, CurrencyType type)
    {
        gameObject.SetActive(true);

        currencyIcon.gameObject.SetActive(true);
        currencyIcon.sprite = PlayerCurrency.GetIcon(type);

        buttonText.text = Cost.ToString();

        if (PlayerCurrency.GetCurrency(type) > Cost)
            button.interactable = true;
        else
            button.interactable = false;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnBuy(type));
    }
    
    public void DisableButton()
    {
        gameObject.SetActive(false);
    }
}
