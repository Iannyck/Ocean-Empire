using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeProposition : MonoBehaviour {

    #region Paramètres

    public Text title;
    public Text description;
    public Button priceButton;
    public Text priceText;

    public int actualPrice = 0;
    public bool isBuy = false;

    #endregion

    public void Start()
    {
        if (CheckError())
            return;

        priceText.text = actualPrice.ToString();
    }

    public void ChangePrice(int price)
    {
        actualPrice = price;
        priceText.text = actualPrice.ToString();
    }

    public void BuyUpgrade()
    {
        if (isBuy)
            return; 

        bool enoughMoney = true; // Verifier le gold du joueur

        if (enoughMoney)
        {
            isBuy = true;
            priceText.text = "Acheté";
        }
    }

    #region CheckException
    private bool CheckError()
    {
        bool errorStatus = false;

        if (title == null)
        {
            Debug.Log("UpgradeProposition ne possède pas de titre");
            errorStatus = true;
        }

        if (description == null)
        {
            Debug.Log("UpgradeProposition ne possède pas de description");
            errorStatus = true;
        }


        if (priceButton == null)
        {
            Debug.Log("UpgradeProposition ne possède pas de PriceButton");
            errorStatus = true;
        }

        if (priceText == null)
        {
            Debug.Log("UpgradeProposition ne possède pas de PriceText");
            errorStatus = true;
        }

        return errorStatus;
    }

    #endregion

};
        
        
