using UnityEngine;
using CCC.Persistence;
using System;

public class PlayerProfile : MonoPersistent
{
    private const string SAVE_KEY_PREFERENCE = "preference";
    private const string SAVE_KEY_LEVEL = "level";

    public static int Level
    {
        get { return instance.level; }
        set { instance.level = value; instance.SavePlayerProfil(); }
    }

    [SerializeField, ReadOnly] private int level;
    [SerializeField] private DataSaver playerProfileSaver;

    public Preferences preferences;
    public static PlayerProfile instance;

    protected void Awake()
    {
        playerProfileSaver.OnReassignData += Refetch;
    }

    public override void Init(Action onComplete)
    {
        instance = this;
        FetchPlayerProfil();
        onComplete();
    }

    public static void IncrementLevel(int value)
    {
        Level = (Level + value).Capped(taskDifficulty.MaxLevel);
    }

    public static void DecrementLevel(int value)
    {
        Level = (Level - value).Raised(0);
    }


    private void SavePlayerProfil()
    {
        playerProfileSaver.SetObjectClone(SAVE_KEY_PREFERENCE, preferences);
        playerProfileSaver.SetInt(SAVE_KEY_LEVEL, level);
        playerProfileSaver.Save();
    }

    private void FetchPlayerProfil()
    {
        preferences = playerProfileSaver.GetObjectClone(SAVE_KEY_PREFERENCE) as Preferences;
        level = playerProfileSaver.GetInt(SAVE_KEY_LEVEL, 0);
    }

    public static void UpdatePlayerLevel(Task completedTask)
    {
        if (taskDifficulty.GetTaskLevel(completedTask) < Level)
            DecrementLevel(1);
        else if (taskDifficulty.GetTaskLevel(completedTask) > Level)
            IncrementLevel(1);
        return;
    }

    public static void Refetch()
    {
        instance.FetchPlayerProfil();
    }
}

