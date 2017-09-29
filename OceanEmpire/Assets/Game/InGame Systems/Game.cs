using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using UnityEngine.SceneManagement;

public delegate void SimpleEvent();

public class Game : PublicSingleton<Game>
{
    static public string SCENENAME = "Recolte_Framework";

    public string mapName;
    public PlayerStats playerStats;
    public PlayerSpawn playerSpawn;

    [HideInInspector]
    public SubmarineMovement submarine;
    [HideInInspector]
    public MapInfo map;
    [HideInInspector]
    public Recolte_UI ui;
    [HideInInspector]
    public bool gameStarted = false;

    public event SimpleEvent OnGameStart;

    public void Start()
    {
        MasterManager.Sync(Init);
    }

    private void Init()
    {
        Scenes.LoadAsync(mapName, LoadSceneMode.Additive, OnMapLoaded);
        Scenes.LoadAsync(Recolte_UI.SCENENAME, LoadSceneMode.Additive, OnUILoaded);
    }


    bool mapLoaded = false;
    bool uiLoaded = false;
    void OnMapLoaded(Scene scene)
    {
        map = scene.FindRootObject<MapInfo>();

        mapLoaded = true;
        CheckStartGame();
    }
    void OnUILoaded(Scene scene)
    {
        ui = scene.FindRootObject<Recolte_UI>();

        uiLoaded = true;
        CheckStartGame();
    }
    void CheckStartGame()
    {
        if (!gameStarted && uiLoaded && mapLoaded)
        {
            StartGame();
        }
    }
    void StartGame()
    {
        gameStarted = true;

        //Spawn player
        submarine = playerSpawn.SpawnFromTop();
        Debug.Log("Game started");

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
