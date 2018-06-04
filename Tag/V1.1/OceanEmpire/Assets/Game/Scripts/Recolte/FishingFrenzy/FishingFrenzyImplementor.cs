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
        if (Game.Instance.IsInFishingFrenzy)
        {
            FishingFrenzyDescription desc = FishingFrenzy.Instance.shopCategory.GetCurrentDescription();
            var mult = desc != null ? desc.fishMultiplier : densityMultiplier;
            Debug.Log("Fish density x" + mult);
            Game.Instance.FishLottery.densityMultiplier *= mult;

            this.DelayedCall(() => popupPrefab.DuplicateGO().Animate(), popupSpawnDelay);
        }
    }
}
