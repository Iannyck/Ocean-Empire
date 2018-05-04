using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shack : MonoBehaviour
{
    public const string SCENENAME = "Shack";

    public CanvasGroup hud;
    public CanvasGroup mainCanvas;

    [Header("Hub")]
    public QuestPanel questPanel;

    [Header("Environement")]
    public Shack_Environment shack_Environment;

    [Header("Recolte")]
    public FishingFrenzyWidget fishingFrenzyWidget;
    public Shack_CallToAction recolteCallToAction;

    [Header("Calendar")]
    public SceneInfo calendarScene;

    void Start()
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            CheckFishingFrenzy();
            if (fishingFrenzyWidget != null)
                fishingFrenzyWidget.OnStateUpdated.AddListener(CheckFishingFrenzy);

            ApplyMapChange(MapManager.Instance.MapData);
            MapManager.Instance.OnMapSet += ApplyMapChange;
        });
    }

    private void ApplyMapChange(int mapIndex, MapData obj) { ApplyMapChange(obj); }
    private void ApplyMapChange(MapData mapData)
    {
        shack_Environment.ApplyMapData(mapData);
    }

    void OnDestroy()
    {
        if (fishingFrenzyWidget != null)
            fishingFrenzyWidget.OnStateUpdated.RemoveListener(CheckFishingFrenzy);

        if (MapManager.Instance != null)
            MapManager.Instance.OnMapSet -= ApplyMapChange;
    }

    void CheckFishingFrenzy()
    {
        recolteCallToAction.enabled = FishingFrenzy.Instance.shopCategory.IsAvailable &&
            FishingFrenzy.Instance.State == FishingFrenzy.EffectState.Available;
    }

    public void OnReturnFromShop()
    {
        fishingFrenzyWidget.UpdateVisibility();
        CheckFishingFrenzy();
        questPanel.UpdateContent();
    }

    public void LaunchGame()
    {
        GameSettings gameSettings = new GameSettings(MapManager.Instance.MapData, true);

        if (FishingFrenzy.Instance != null
            && FishingFrenzy.Instance.shopCategory.IsAvailable
            && FishingFrenzy.Instance.State == FishingFrenzy.EffectState.Available)
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