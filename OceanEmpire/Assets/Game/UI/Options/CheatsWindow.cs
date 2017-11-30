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
        PlayerProfile.ResetPlayerProfil();
    }


    public void ShowMeTheMoney()    //Cheat code StarCraft xD
    {
        if (PlayerCurrency.instance != null)
        {
            PlayerCurrency.AddCoins(200);
        }
    }

    public void UnlockAll()    //Cheat code StarCraft xD
    {
        if (ItemsList.instance != null)
            ItemsList.UnlockAll();
    }
}
