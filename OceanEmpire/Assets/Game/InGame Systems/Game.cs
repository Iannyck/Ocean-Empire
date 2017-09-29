using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using UnityEngine.SceneManagement;

public delegate void SimpleEvent();

public class Game : PublicSingleton<Game>
{
    // GAME COMPONENT
    public static CameraMouvement CameraMouvement { get { return instance.cameraMouvement; } }
    public static PlayerStats PlayerStats { get { return instance.playerStats; } }
    public static PlayerSpawn PlayerSpawn { get { return instance.playerSpawn; } }
    public static Spawner Spawner { get { return instance.spawner; } }

    [SerializeField]
    private Spawner spawner;
    [SerializeField]
    private CameraMouvement cameraMouvement;
    [SerializeField]
    private PlayerStats playerStats;
    [SerializeField]
    private PlayerSpawn playerSpawn;

    [HideInInspector]
    public SubmarineMovement submarine;
    [HideInInspector]
    public MapInfo map;
    [HideInInspector]
    public Recolte_UI ui;

    // GAME STATE
    [HideInInspector]
    public bool gameStarted = false;
    [HideInInspector]
    public bool gameReady = false;
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
        cameraMouvement.followPlayer = false;

        //Spawn player
        submarine = playerSpawn.SpawnPlayer();

        //Ready up !
        ReadyGame();

        //Intro
        playerSpawn.AnimatePlayerFromTop(submarine,
            delegate ()
            {
                cameraMouvement.followPlayer = true;
                playerSpawned = true;
                StartGame();
            });
    }

    void ReadyGame()
    {
        if (gameReady)
            return;

        gameReady = true;

        LoadingScreen.OnNewSetupComplete();

        if (OnGameReady != null)
        {
            OnGameReady();
            OnGameReady = null;
        }
    }

    void StartGame()
    {
        if (gameStarted || !playerSpawned)
            return;

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
