﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

using CCC.UI;

public class ConfirmBuy : WindowAnimation
{
    public const string SCENENAME = "ShopConfirm";

    Action<bool> boughtConfirm;

    //  public Text itemName;

    private int price;

    //  public Text cost;
    //public Image CurrencyImage;

    public static void OpenWindowAndConfirm(CurrencyType currency, Action<bool> resultCallback)
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            if (Scenes.IsActive(SCENENAME))
            {
                Scenes.GetActive(SCENENAME).FindRootObject<ConfirmBuy>().Init(currency, resultCallback);
            }
            else
            {
                Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
                {
                    scene.FindRootObject<ConfirmBuy>().Init(currency, resultCallback);
                });
            }
        });
    }


    public void Init(CurrencyType currency, Action<bool> resultCallback)
    {
        //itemName.text = itemD.GetName();
        boughtConfirm = resultCallback;
        // if (coinCurrency)
        // {
        //     price = itemD.GetMoneyCost();
        //     CurrencyImage.sprite = PlayerCurrency.GetMoneyIcon();
        // }

        //  else
        //  {
        //      price = itemD.GetTicketCost();
        //      CurrencyImage.sprite = PlayerCurrency.GetTicketIcon();
        //  }
        //  cost.text = price.ToString();
    }

    public void OnConfirmClick()
    {
        if (boughtConfirm == null)
            Close(() => Scenes.UnloadAsync(SCENENAME));
        else
        {
            boughtConfirm(true);
            Close(() => Scenes.UnloadAsync(SCENENAME));
        }
    }
    public void OnCancelClick()
    {
        if (boughtConfirm == null)
            Close(() => Scenes.UnloadAsync(SCENENAME));
        else
        {
            boughtConfirm(false);
            Close(() => Scenes.UnloadAsync(SCENENAME));
        }
    }
}
