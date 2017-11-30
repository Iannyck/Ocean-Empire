using CCC.Manager;
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
        MasterManager.Sync();
    }


    public void ResetDataToDefault()
    {
        if (GameSaves.instance != null)
            GameSaves.instance.ClearAllSaves();

        if (ItemsList.instance != null)
            ItemsList.Reload();

        if (PlayerCurrency.instance != null)
            PlayerCurrency.Reload();

        if (FishPopulation.instance != null)
            FishPopulation.Reload();

        ActivityDetection.ResetActivitiesSave();
        ActivityAnalyser.instance.ResetActivities();
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

    public void UnlockAll()    //Cheat code StarCraft xD
    {
        if (ItemsList.instance != null)
            ItemsList.UnlockAll();
    }
}
