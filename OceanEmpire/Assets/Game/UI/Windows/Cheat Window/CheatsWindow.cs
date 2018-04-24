 
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatsWindow : WindowAnimation
{
    public const string SCENENAME = "CheatsWindow";

    public static void OpenWindow()
    {
        Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive);
    }

    protected override void Awake()
    {
        base.Awake();
        PersistentLoader.LoadIfNotLoaded();
    }


    public void ResetDataToDefault()
    {
        var dataSavers = DataSaverBank.Instance.GetDataSavers();
        foreach (DataSaver saver in dataSavers)
        {
            saver.ClearSave();
        }

        GoogleReader.ResetActivitiesSave();
        GoogleActivities.instance.ResetActivities();
    }


    public void ShowMeTheMoney(int amount)    //Cheat code StarCraft xD
    {
        if (PlayerCurrency.instance != null)
        {
            if (amount < 0)
                PlayerCurrency.RemoveCoins(amount.Abs());
            else
                PlayerCurrency.AddCoins(amount);
        }
    }
    public void GiveTickets(int amount)    //Cheat code StarCraft xD
    {
        if (PlayerCurrency.instance != null)
        {
            if (amount < 0)
                PlayerCurrency.RemoveTickets(amount.Abs());
            else
                PlayerCurrency.AddTickets(amount);
        }
    }
}
