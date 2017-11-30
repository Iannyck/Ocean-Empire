using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;
using CCC.Manager;

[System.Serializable]
public class PlayerProfile : BaseManager<PlayerProfile>
{
    private const string SAVE_KEY_PREFERENCE = "preference";
    private const string SAVE_KEY_LEVEL = "level";

    public static int Level
    {
        get { return instance.level; }
        set { instance.level = value; instance.SavePlayerProfil();}
    }

    [SerializeField, ReadOnly]
    private int level;

    public Preferences preferences;

    public static void IncrementLevel(int value)
    {
        Level = (Level + value).Capped(taskDifficulty.MaxLevel); 
    }

    public static void DecrementLevel(int value)
    {
        Level = (Level - value).Raised(0);
    }

    public override void Init()
    {
        LoadPlayerProfil();
        CompleteInit();
    }


    private void SavePlayerProfil()
    {
        GameSaves.instance.SetObject(GameSaves.Type.PlayerProfile, SAVE_KEY_PREFERENCE, preferences);
        GameSaves.instance.SetInt(GameSaves.Type.PlayerProfile, SAVE_KEY_LEVEL, level);
        GameSaves.instance.SaveData(GameSaves.Type.PlayerProfile);
    }

    private void LoadPlayerProfil()
    {
        preferences = GameSaves.instance.GetObject(GameSaves.Type.PlayerProfile, SAVE_KEY_PREFERENCE) as Preferences;
        level = GameSaves.instance.GetInt(GameSaves.Type.PlayerProfile, SAVE_KEY_LEVEL, 0);
    }

    
    public static void Reload()
    {
        instance.LoadPlayerProfil();
    }
    public static void ResetPlayerProfil()
    {
        GameSaves.instance.ClearPlayerProfile();
        Reload();
    }
}

