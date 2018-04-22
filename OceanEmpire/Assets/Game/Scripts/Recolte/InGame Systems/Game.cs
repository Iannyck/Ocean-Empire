using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.DesignPattern;
using System;

public delegate void SimpleEvent();

public class Game : PublicSingleton<Game>
{
    // GAME COMPONENT
    public GameBuilder GameBuilder { get { return _gameBuilder; } }
    public FishSpawner FishSpawner { get { return _fishSpawner; } }
    public PalierManager PalierManager { get { return _palierManager; } }
    public GPComponents.SceneManager SceneManager { get { return _sceneManager; } }
    public SubmarinParts SubmarinParts { get { return SubmarineMovement == null ? null : SubmarineMovement.GetComponent<SubmarinParts>(); } }
    public PlayerStats PlayerStats { get { return _playerStats; } }
    public PlayerSpawn PlayerSpawn { get { return _playerSpawn; } }
    public UnitInstantiator UnitInstantiator { get { return _instantiator; } }
    public GameCamera GameCamera { get { return _gameCamera; } }
    public GameObject Submarine { get { return SubmarineMovement.gameObject; } }
    public MapBuilder MapBuilder { get { return _mapBuilder; } }
    public Shack_Environment Shack_Environment { get { return _shack_Environment; } }
    public MapLayout MapLayout { get { return _mapLayout; } }

    public GameSettings GameSettings { get; private set; }
    public Recolte_UI Recolte_UI { get; private set; }
    public SubmarineMovement SubmarineMovement { get; private set; }
    public FishingReport FishingReport { get; private set; }
    public FishLottery FishLottery { get; private set; }
    public PendingFishGPC PendingFishGPC { get; private set; }
    public Locker GameRunning { get; private set; }

    [SerializeField] UnitInstantiator _instantiator;
    [SerializeField] PlayerStats _playerStats;
    [SerializeField] PlayerSpawn _playerSpawn;
    [SerializeField] GameCamera _gameCamera;
    [SerializeField] GPComponents.SceneManager _sceneManager;
    [SerializeField] PalierManager _palierManager;
    [SerializeField] FishSpawner _fishSpawner;
    [SerializeField] GameBuilder _gameBuilder;
    [SerializeField] MapBuilder _mapBuilder;
    [SerializeField] Shack_Environment _shack_Environment;
    [SerializeField] MapLayout _mapLayout;

    // GAME STATE
    [NonSerialized] public bool gameStarted = false;
    [NonSerialized] public bool gameReady = false;
    [NonSerialized] public bool gameOver = false;
    [NonSerialized] bool playerSpawned = false;

    static private event SimpleEvent onGameReady;
    static private event SimpleEvent onGameStart;

    protected override void Awake()
    {
        base.Awake();

        GameRunning = new Locker();
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

    public void InitGame(GameSettings gameSettings)
    {
        this.GameSettings = gameSettings;

        if (DragThreashold.instance != null)
            DragThreashold.instance.SetDragType(DragThreashold.DragType.InGame);

        _gameCamera.CameraMovement.followPlayer = false;
        Time.timeScale = 1;
        GameRunning.onLockStateChange += GameRunning_onLockStateChange;

        //Create a fishingReport
        FishingReport = new FishingReport();

        //Spawn player
        SubmarineMovement = _playerSpawn.SpawnPlayer();
        SubmarineMovement.canAccelerate.Lock("game");

        // Build map
        if (MapBuilder != null)
            MapBuilder.ApplyMapData(GameSettings.MapData);

        //Init fish lottery
        if (FishLottery != null)
            FishLottery.Init(MapLayout);

        // TEMPORAIRE
        if (Shack_Environment != null)
            Shack_Environment.ApplyMapData(GameSettings.MapData);

        //Ready up !
        ReadyGame();

        //Intro
        _playerSpawn.AnimatePlayerIntro(SubmarineMovement,
            delegate ()
            {
                _gameCamera.CameraMovement.followPlayer = true;
                SubmarineMovement.canAccelerate.Unlock("game");
                playerSpawned = true;
                Recolte_UI.feedbacks.ShowGo(null);
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

        _gameCamera.CameraMovement.followPlayer = false;
        SubmarineMovement.canAccelerate.Lock("end");
        _playerSpawn.AnimatePlayerExit(SubmarineMovement, () =>
        {
            LoadingScreen.TransitionTo(FishingSummary.SCENENAME, new ToFishingSummaryMessage(FishingReport));
        });
    }

    #region Other Scene Reference
    public void SetReference(FishLottery fishLottery)
    {
        FishLottery = fishLottery;
    }
    public void SetReference(Recolte_UI recolte_UI)
    {
        Recolte_UI = recolte_UI;
    }
    #endregion
}
