using UnityEngine;
using CCC.Persistence;
using System;

public class PlayerProfile : MonoPersistent
{
    private const string SAVE_KEY_LEVEL = "level";
    private const string SAVE_KEY_LOG = "log";
    private const string SAVE_KEY_REPORT = "report";

    [SerializeField] private DataSaver playerProfileSaver;

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


    private void SavePlayerProfil()
    {
        //Set data into playerProfileSaver
        //playerProfileSaver.Save();
    }

    private void FetchPlayerProfil()
    {
        //Get data from playerProfileSaver
    }

    public static void Refetch()
    {
        instance.FetchPlayerProfil();
    }

    public void Log(string description)
    {
        playerProfileSaver.SetString(SAVE_KEY_LOG, description);
        playerProfileSaver.LateSave();
    }

    public void LogReport(ExerciseReport report)
    {
        playerProfileSaver.SetString(SAVE_KEY_REPORT, report.GetString());
        playerProfileSaver.LateSave();
    }
}

