using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.DesignPattern;

public delegate void SimpleEvent();

public class Game : PublicSingleton<Game>
{
    // GAME COMPONENT
    public static CameraMouvement CameraMouvement { get { return instance.cameraMouvement; } }
    public static PlayerStats PlayerStats { get { return instance.playerStats; } }
    public static PlayerSpawn PlayerSpawn { get { return instance.playerSpawn; } }
    public static FishingReport FishingReport { get { return instance.fishingReport; } }
    public static UnitInstantiator UnitInstantiator { get { return instance.instantiator; } }
    public static GameCamera GameCamera { get { return instance.gameCamera; } }
    public static Recolte_UI Recolte_UI { get { return instance.ui; } }
    public static SubmarineMovement SubmarineMouvement { get { return instance.submarine; } }
    public static GameObject Submarine { get { return instance.submarine.gameObject; } }
    public static SubmarinParts SubmarinParts { get { return (instance.submarine == null ? null : instance.submarine.gameObject.GetComponent<SubmarinParts>()); } }
    public static GPComponents.SceneManager SceneManager { get { return instance.sceneManager; } }
    public static PalierManager PalierManager { get { return instance.palierManager; } }
    public static FishSpawner FishSpawner { get { return instance.fishSpawner; } }

    [SerializeField] private UnitInstantiator instantiator;
    [SerializeField] private CameraMouvement cameraMouvement;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerSpawn playerSpawn;
    [SerializeField] private GameCamera gameCamera;
    [SerializeField] private GPComponents.SceneManager sceneManager;
    [SerializeField] private PalierManager palierManager;
    [SerializeField] private FishSpawner fishSpawner;

    [HideInInspector] public SubmarineMovement submarine;
    [HideInInspector] public FishingReport fishingReport;
    [HideInInspector] public MapInfo map;
    [HideInInspector] public Recolte_UI ui;
    public PendingFishGPC PendingFishGPC { get; private set; }

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

    [HideInInspector]
    public Locker gameRunning = new Locker();


    protected override void Awake()
    {
        base.Awake();

        PendingFishGPC = new PendingFishGPC();
    }


    static public event SimpleEvent OnGameReady
    {
        add
        {
            if (instance != null && instance.gameReady)
                value();
            else
                onGameReady += value;
        }
        remove { onGameReady -= value; }
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
        remove { onGameStart -= value; }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onGameReady = null;
        onGameStart = null;

        if (DragThreashold.instance != null)
            DragThreashold.instance.SetDragType(DragThreashold.DragType.InMenu);
    }

    public void InitGame()
    {
        if (DragThreashold.instance != null)
            DragThreashold.instance.SetDragType(DragThreashold.DragType.InGame);

        cameraMouvement.followPlayer = false;
        Time.timeScale = 1;
        gameRunning.onLockStateChange += GameRunning_onLockStateChange;

        //Create a fishingReport
        fishingReport = new FishingReport();

        //Spawn player
        submarine = playerSpawn.SpawnPlayer();
        submarine.canAccelerate.Lock("game");

        //Ready up !
        ReadyGame();

        //Intro
        playerSpawn.AnimatePlayerIntro(submarine,
            delegate ()
            {
                cameraMouvement.followPlayer = true;
                submarine.canAccelerate.Unlock("game");
                playerSpawned = true;
                ui.feedbacks.ShowGo(null);
                StartGame();
            });
    }

    private void GameRunning_onLockStateChange(bool state)
    {
        if (state)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
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
        if (gameOver)
            return;
        gameOver = true;

        cameraMouvement.followPlayer = false;
        submarine.canAccelerate.Lock("end");
        playerSpawn.AnimatePlayerExit(submarine);

        ui.feedbacks.ShowTimeUp(delegate ()
        {
            LoadingScreen.TransitionTo(FishingSummary.SCENENAME, new ToFishingSummaryMessage(fishingReport));
        });
    }
}
