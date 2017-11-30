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
    public float rewardScale = 1;

    public Preferences preferences;

    public static void IncrementLevel(int value)
    {
        Level = (Level + value).Capped(TaskDifficulty.MaxLevel); 
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
        GameSaves.instance.SetObjectClone(GameSaves.Type.PlayerProfile, SAVE_KEY_PREFERENCE, preferences);
        GameSaves.instance.SetInt(GameSaves.Type.PlayerProfile, SAVE_KEY_LEVEL, level);
        GameSaves.instance.SaveData(GameSaves.Type.PlayerProfile);
    }

    private void LoadPlayerProfil()
    {
        preferences = GameSaves.instance.GetObjectClone(GameSaves.Type.PlayerProfile, SAVE_KEY_PREFERENCE) as Preferences;
        level = GameSaves.instance.GetInt(GameSaves.Type.PlayerProfile, SAVE_KEY_LEVEL, 0);
    }

    public static void updatePlayerLevel(Task completedTask)
    {
        if (taskDifficulty.GetTaskLevel(completedTask) < Level)
            DecrementLevel(1);
        else if (taskDifficulty.GetTaskLevel(completedTask) > Level)
            IncrementLevel(1);
        return;
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

