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
    public static FishSpawner FishSpawner { get { return instance.fishSpawner; } }
    public static FishingReport FishingReport { get { return instance.fishingReport; } }
    public static Spawner Spawner { get { return instance.spawner; } }
    public static GameCamera GameCamera { get { return instance.gameCamera; } }
    public static Recolte_UI Recolte_UI { get { return instance.ui; } }

    public static SubmarineMovement SubmarineMouvement { get { return instance.submarine; } }
    public static SubmarinParts SubmarinParts { get { return (instance.submarine == null ? null : instance.submarine.gameObject.GetComponent<SubmarinParts>()); } }

    [SerializeField]
    private Spawner spawner;
    [SerializeField]
    private CameraMouvement cameraMouvement;
    [SerializeField]
    private PlayerStats playerStats;
    [SerializeField]
    private PlayerSpawn playerSpawn;
    [SerializeField]
    private FishSpawner fishSpawner;
    [SerializeField]
    private GameCamera gameCamera;

    [HideInInspector]
    public SubmarineMovement submarine;
    [HideInInspector]
    public FishingReport fishingReport;
    [HideInInspector]
    public MapInfo map;
    [HideInInspector]
    public Recolte_UI ui;

    // GAME STATE
    [HideInInspector]
    public bool gameStarted = false;
    [HideInInspector]
    public bool gameReady = false;
    [HideInInspector]
    public bool gameOver = false;
    bool playerSpawned = false;
    static private event SimpleEvent onGameReady;
    static private event SimpleEvent onGameStart;

    static public event SimpleEvent OnGameReady
    {
        add
        {
            if (instance != null && instance.gameReady)
                value();
            else
                onGameReady += value;
        }
        remove
        {
            onGameReady -= value;
        }
    }

    static public event SimpleEvent OnGameStart
    {
        add
        {
            if (instance != null && instance.gameStarted)
                value();
            else
                onGameStart += value;
        }
        remove
        {
            onGameStart -= value;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onGameReady = null;
        onGameStart = null;
    }

    public void InitGame()
    {
        cameraMouvement.followPlayer = false;
        Time.timeScale = 1;

        //Create a fishingReport
        fishingReport = new FishingReport();

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
                ui.feedbacks.ShowGo(StartGame);
            });
    }

    void ReadyGame()
    {
        if (gameReady)
            return;

        gameReady = true;

        LoadingScreen.OnNewSetupComplete();

        if (onGameReady != null)
        {
            onGameReady();
            onGameReady = null;
        }
    }

    void StartGame()
    {
        if (gameStarted || !playerSpawned)
            return;

        gameStarted = true;

        Debug.Log("Game started");

        // Init Game Start Events
        if (onGameStart != null)
        {
            onGameStart();
            onGameStart = null;
        }
    }

    public void EndGame()
    {
        // End Game Screen
        // Save All
        if (gameOver)
            return;
        gameOver = true;
        ui.feedbacks.ShowTimeUp(delegate ()
        {
            LoadingScreen.TransitionTo(FishingSummary.SCENENAME, new ToFishingSummaryMessage(fishingReport));
        });
    }
}
