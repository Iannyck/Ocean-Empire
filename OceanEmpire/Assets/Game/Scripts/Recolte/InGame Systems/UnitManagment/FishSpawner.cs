using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [Header("References"), SerializeField] private UnitInstantiator instantiator;
    public UnitInstantiator Instantiator { get { return instantiator; } private set { instantiator = value; } }

    [SerializeField] private PalierManager palierManager;
    public PalierManager PalierManager { get { return palierManager; } private set { palierManager = value; } }

    [Header("Settings"), SerializeField]
    private int fishPerPalier = 2;

    private GameCamera Camera { get { return Game.GameCamera; } }
    private FishLottery FishLottery { get { return Game.Instance.fishLottery; } }
    private PendingFishGPC PendingGPC { get { return Game.Instance.PendingFishGPC; } }

    void Start()
    {
        Game.OnGameStart += OnGameStart;
    }

    void OnEnable()
    {
        if (PalierManager != null && Game.Instance != null && Game.Instance.gameStarted)
            PalierManager.OnPalierActivated += OnPalierActivated;
    }

    void OnDisable()
    {
        if (PalierManager != null)
            PalierManager.OnPalierActivated -= OnPalierActivated;
    }

    private void OnGameStart()
    {
        var paliers = PalierManager.GetPalier();
        for (int i = 0; i < paliers.Count; i++)
        {
            if (!paliers[i].IsActive)
                continue;
            SpawnOnEdgesOfPalier(paliers[i].Index, fishPerPalier);
            paliers[i].HasBeenSeen.FlipValue();
        }

        PalierManager.OnPalierActivated += OnPalierActivated;
    }

    void OnPalierActivated(Palier palier)
    {
        // New Fish
        if (!palier.HasBeenSeen)
        {
            SpawnWithinPalier(palier.Index, fishPerPalier);
            palier.HasBeenSeen.FlipValue();
        }

        // Old fish that killed themselves in the palier
        SpawnWithinPalier(palier.Index, palier.FishToRespawn);
        palier.FishToRespawn = 0;
    }

    void SpawnOnEdgesOfPalier(int palier, int count)
    {
        for (int i = 0; i < count; i++)
            SpawnOnEdgesOfPalier(palier);
    }
    void SpawnOnEdgesOfPalier(int palier)
    {
        var plans = PalierManager.PalierPlans;
        float horizontalOffset = Camera.HalfWidth * 1.2f;
        Vector2 position = new Vector2(
            Random.value > 0.5f ? horizontalOffset : -horizontalOffset,
            (Random.value - 0.5f) * plans.PalierThickness + plans.GetPalierCenter(palier));

        SpawnAt(position);
    }

    void SpawnWithinPalier(int palier, int count)
    {
        for (int i = 0; i < count; i++)
            SpawnWithinPalier(palier);
    }
    void SpawnWithinPalier(int palier)
    {
        var plans = PalierManager.PalierPlans;
        Vector2 position = new Vector2(
            (Random.value - 0.5f) * Camera.Width,
            (Random.value - 0.5f) * plans.PalierThickness + plans.GetPalierCenter(palier));

        SpawnAt(position);
    }

    void SpawnAt(Vector2 position)
    {
        var fish = FishLottery.DrawAtHeight(position.y);
        if (fish == null)
            return;

        // Melee capture GPC
        var meleeCapturable = fish.GetComponent<MeleeCapturable>();
        if (meleeCapturable != null)
        {
            PendingGPC.AddPendingFishGPC(new GPComponents.GPC_MeleeCapture(Game.SceneManager, meleeCapturable, position));
            return;
        }

        // Harpoon capture GPC
        var harpoonCapturable = fish.GetComponent<HarpoonCapturable>();
        if (harpoonCapturable != null)
        {
            PendingGPC.AddPendingFishGPC(new GPComponents.GPC_HarpoonCapture(Game.SceneManager, harpoonCapturable, position));
            return;
        }

        // Not eligible for a GPC
        Debug.LogWarning("Spawned a fish that was not elligible for any type of GPC");
        Instantiator.Spawn(fish, position);
    }
}
