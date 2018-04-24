using UnityEngine;
using CCC.Persistence;
using System;

public class PlayerProfile : MonoPersistent
{
    private const string SAVE_KEY_LEVEL = "level";

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

    public void Refetch()
    {
        FetchPlayerProfil();
    }
}

