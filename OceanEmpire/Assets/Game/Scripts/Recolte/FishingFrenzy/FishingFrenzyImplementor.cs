using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingFrenzyImplementor : MonoBehaviour
{
    public float densityMultiplier = 1.5f;

    [SerializeField] FishingFrenzyActivatedPopup popupPrefab;
    [SerializeField] float popupSpawnDelay = 1f;

    void OnEnable()
    {
        Game.OnGameReady += OnGameReady;
    }

    void OnDisable()
    {
        Game.OnGameReady -= OnGameReady;
    }

    void OnGameReady()
    {
        if (Game.Instance.GameSettings.CanUseFishingFrenzy &&
            FishingFrenzy.Instance != null &&
            FishingFrenzy.Instance.State == FishingFrenzy.EffectState.CurrentlyActive)
        {
            Debug.Log("Fish density x" + densityMultiplier);
            Game.Instance.FishLottery.densityMultiplier *= densityMultiplier;

            this.DelayedCall(() => popupPrefab.DuplicateGO().Animate(), popupSpawnDelay);
        }
    }
}
