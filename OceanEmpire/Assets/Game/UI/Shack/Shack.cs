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
    [SerializeField] Shack_MapManager shack_MapManager;

    [Header("Calendar")]
    [SerializeField] SceneInfo calendarScene;

    public void OpenCalendar()
    {
        Scenes.Load(calendarScene, (scene) =>
        {
            scene.FindRootObject<CalendarScroll_Controller>().OnEntranceComplete(() => Scenes.UnloadAsync(gameObject.scene));
        });
    }

    void OnEnable()
    {
        CheckFishingFrenzy();
        if (fishingFrenzyWidget != null)
            fishingFrenzyWidget.OnStateUpdated.AddListener(CheckFishingFrenzy);

        shack_MapManager.OnMapSet += OnMapChange;
    }

    private void OnMapChange(int mapIndex, MapData obj)
    {
        shack_Environment.ApplyMapData(obj);
    }

    void OnDisable()
    {
        if (fishingFrenzyWidget != null)
            fishingFrenzyWidget.OnStateUpdated.RemoveListener(CheckFishingFrenzy);

        if (shack_MapManager != null)
            shack_MapManager.OnMapSet -= OnMapChange;
    }

    void CheckFishingFrenzy()
    {
        recolteCallToAction.enabled = FishingFrenzy.Instance.State == FishingFrenzy.EffectState.Available;
    }

    public void LaunchGame()
    {
        GameSettings gameSettings = new GameSettings(shack_MapManager.MapData, true);

        if (FishingFrenzy.Instance != null && FishingFrenzy.Instance.State == FishingFrenzy.EffectState.Available)
        {
            FishingFrenzy.Instance.Activate();
        }
        LoadingScreen.TransitionTo(GameBuilder.SCENENAME, new ToRecolteMessage(gameSettings), true);
    }
}
