using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shack : MonoBehaviour
{
    public const string SCENENAME = "Shack";

    [Header("Components")]
    [SerializeField]
    SceneInfo calendarScene;
    [SerializeField] Shack_CallToAction recolteCallToAction;
    [SerializeField] FishingFrenzyWidget fishingFrenzyWidget;

    public PrebuiltMapData defaultMap;
    public MapData MapData { get; private set; }

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
    }

    void OnDisable()
    {
        if (fishingFrenzyWidget != null)
            fishingFrenzyWidget.OnStateUpdated.RemoveListener(CheckFishingFrenzy);
    }

    void CheckFishingFrenzy()
    {
        recolteCallToAction.enabled = FishingFrenzy.Instance.State == FishingFrenzy.EffectState.Available;
    }

    public void LaunchGame()
    {
        var mapData = MapData ?? defaultMap.MapData;
        GameSettings gameSettings = new GameSettings(mapData.GameSceneName, true);

        if(FishingFrenzy.Instance != null && FishingFrenzy.Instance.State == FishingFrenzy.EffectState.Available)
        {
            FishingFrenzy.Instance.Activate();
        }
        LoadingScreen.TransitionTo(GameBuilder.SCENENAME, new ToRecolteMessage(gameSettings), true);
    }
}
