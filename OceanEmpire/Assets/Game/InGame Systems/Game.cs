using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using UnityEngine.SceneManagement;

public delegate void SimpleEvent();

public class Game : PublicSingleton<Game>
{
    // GAME COMPONENT
    public CameraMouvement cameraMouvement;
    public PlayerStats playerStats;
    public PlayerSpawn playerSpawn;
    [HideInInspector]
    public SubmarineMovement submarine;
    [HideInInspector]
    public MapInfo map;
    [HideInInspector]
    public Recolte_UI ui;

    // GAME STATE
    [HideInInspector]
    public bool gameStarted = false;
    bool playerSpawned = false;
    static public event SimpleEvent OnGameReady;
    static public event SimpleEvent OnGameStart;

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnGameReady = null;
        OnGameStart = null;
    }

    public void InitGame()
    {
        //Spawn player
        cameraMouvement.followPlayer = false;
        submarine = playerSpawn.SpawnFromTop(delegate ()
        {
            cameraMouvement.followPlayer = true;
            playerSpawned = true;
            CheckStartGame();
        });

        // Init other things in game
        // if Async put another CheckStartGame

        CheckStartGame();
    }

    void CheckStartGame()
    {
        if (!gameStarted && playerSpawned)
        {
            if(OnGameReady != null)
            {
                OnGameReady();
                OnGameReady = null;
            }
            StartGame();
        }
    }

    void StartGame()
    {
        gameStarted = true;

        Debug.Log("Game started");

        // Init Game Start Events
        if (OnGameStart != null)
        {
            OnGameStart();
            OnGameStart = null;
        }
    }

    public void End()
    {
        // End Game Screen
        // Save All
        LoadingScreen.TransitionTo("Shack_Map", null);
    }
}
