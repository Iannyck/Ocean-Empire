using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shack : MonoBehaviour
{
    public const string SCENENAME = "Shack";

    [Header("Environement")]
    [SerializeField] Shack_Environment shack_Environment;

    [Header("Recolte")]
    [SerializeField] FishingFrenzyWidget fishingFrenzyWidget;
    [SerializeField] Shack_CallToAction recolteCallToAction;

    [Header("Calendar")]
    [SerializeField] SceneInfo calendarScene;

    void Start()
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            CheckFishingFrenzy();
            if (fishingFrenzyWidget != null)
                fishingFrenzyWidget.OnStateUpdated.AddListener(CheckFishingFrenzy);

            MapManager.Instance.OnMapSet += OnMapChange;
        });
    }

    private void OnMapChange(int mapIndex, MapData obj)
    {
        shack_Environment.ApplyMapData(obj);
    }

    void OnDestroy()
    {
        if (fishingFrenzyWidget != null)
            fishingFrenzyWidget.OnStateUpdated.RemoveListener(CheckFishingFrenzy);

        if (MapManager.Instance != null)
            MapManager.Instance.OnMapSet -= OnMapChange;
    }

    void CheckFishingFrenzy()
    {
        recolteCallToAction.enabled = FishingFrenzy.Instance.State == FishingFrenzy.EffectState.Available;
    }

    public void LaunchGame()
    {
        GameSettings gameSettings = new GameSettings(MapManager.Instance.MapData, true);

        if (FishingFrenzy.Instance != null && FishingFrenzy.Instance.State == FishingFrenzy.EffectState.Available)
        {
            FishingFrenzy.Instance.Activate();
        }
        LoadingScreen.TransitionTo(GameBuilder.SCENENAME, new ToRecolteMessage(gameSettings), true);
    }

    public void OpenCalendar()
    {
        Scenes.Load(calendarScene, (scene) =>
        {
            scene.FindRootObject<CalendarScroll_Controller>().OnEntranceComplete(() => Scenes.UnloadAsync(gameObject.scene));
        });
    }
}
