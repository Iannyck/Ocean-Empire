using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using CCC.Manager;
using CCC.UI;

public class ConfirmBuy : WindowAnimation
{
    public const string SCENENAME = "ShopConfirm";

    Action<bool> boughtConfirm;

  //  public Text itemName;

    private int price;

  //  public Text cost;
//public Image CurrencyImage;

    public static void OpenWindowAndConfirm(ItemDescription item, CurrencyType currency, Action<bool> resultCallback)
    {
        print("ping!");
        MasterManager.Sync(() =>
        {
            if (Scenes.IsActive(SCENENAME))
            {
                Scenes.GetActive(SCENENAME).FindRootObject<ConfirmBuy>().Init(item, currency, resultCallback);
            }
            else
            {
                Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
                {
                    scene.FindRootObject<ConfirmBuy>().Init(item, currency, resultCallback);
                });
            }
        });
    }

    public void Init(ItemDescription itemD, CurrencyType currency, Action<bool> resultCallback)
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
