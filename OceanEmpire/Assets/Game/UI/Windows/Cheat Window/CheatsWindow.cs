
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheatsWindow : WindowAnimation
{
    [Header("CHEATS")]
    [SerializeField] int mapIndex;
    [SerializeField] Text setMapText;
    [SerializeField] DataSaver[] dataSavers;

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

    private void Update()
    {
        if (setMapText != null)
        {
            setMapText.text = "Set Map (index: " + mapIndex + ')';
        }
    }

    public void ClearSave()
    {
        foreach (DataSaver saver in dataSavers)
        {
            if (saver != null)
                saver.ClearSave();
        }

        GoogleReader.ResetActivitiesSave();
        GoogleActivities.instance.ResetActivities();

        Scenes.Load("Intro", LoadSceneMode.Single);
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

    public void SetMap(bool withQuest)
    {
        MapManager.Instance.SetMap(mapIndex, withQuest);
    }

    public void NextMap(bool withQuests)
    {
        MapManager.Instance.SetMap_Next(withQuests);
    }

    public void PreviousMap(bool withQuests)
    {
        MapManager.Instance.SetMap_Previous(withQuests);
    }
}
