using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shack : MonoBehaviour
{
    public const string SCENENAME = "Shack";

    [Header("Components")]
    [SerializeField] SceneInfo calendarScene;
    [SerializeField] Shack_CallToAction recolteCallToAction;
    [SerializeField] FishingFrenzyWidget fishingFrenzyWidget;

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
}
