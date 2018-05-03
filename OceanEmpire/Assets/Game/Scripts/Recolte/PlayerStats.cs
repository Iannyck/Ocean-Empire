using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float depthRecord;
    public event Action<Capturable, CaptureTechnique> OnCapture;

    public void TryCapture(Capturable fish, CaptureTechnique captureTechnique)
    {
        fish.Capture();
        CallCoinsPopUp(fish.info);

        if (captureTechnique == CaptureTechnique.Harpoon)
        {
            HarpoonThrowerDescription harpoonThrowerDescription = Game.Instance.SubmarinParts.HarpoonThrower.Description;
            if (harpoonThrowerDescription != null && harpoonThrowerDescription.bonusCoins > 0)
            {
                var position = fish.transform.position;
                var offset = position.x > 0 ? Vector3.left * 0.75f : Vector3.right * 0.75f;
                CallCoinsPopUp(harpoonThrowerDescription.bonusCoins, position + offset);
                Game.Instance.FishingReport.harpoonBonusGold += harpoonThrowerDescription.bonusCoins;
                Game.Instance.FishingReport.harpoonBonusCount++;
            }
        }

        if (OnCapture != null)
            OnCapture(fish, captureTechnique);
    }

    private void CallCoinsPopUp(FishInfo info) { CallCoinsPopUp(info, Vector3.zero); }
    private void CallCoinsPopUp(FishInfo info, Vector3 offset)
    {
        int fishWorth = info.description.baseMonetaryValue.RoundedToInt();
        Vector3 spawnPos = info.transform.position + offset;
        CallCoinsPopUp(fishWorth, spawnPos);
    }
    private void CallCoinsPopUp(int coins, Vector3 worldPosition)
    {
        Game.Instance.Recolte_UI.GetComponent<SpawnCoinsPopUp>().SpawnPopUp(worldPosition, coins);
    }
}
