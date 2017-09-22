using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;

public class Game : PublicSingleton<Game>
{
    public MapInfo map;
    public PlayerStats player;
    public PlayerSpawn playerSpawn;

    public float gameDuration = 10;

    public void Start()
    {
        Debug.Log("Game Init");
        if(GetComponent<MapInfo>() != null)
            map = GetComponent<MapInfo>();
        MasterManager.Sync();
        playerSpawn.SpawnFromTop();
        DelayManager.LocalCallTo(End, gameDuration, this);
    }

    public void End()
    {
        // End Game Screen
        // Save All
        LoadingScreen.TransitionTo("Shack_Map", null);
    }
}
