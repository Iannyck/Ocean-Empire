using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float depthRecord;

    public void TryCapture(Capturable fish)
    {
        fish.Capture();
        CallCoinsPopUp(fish.info);
    }

    private void CallCoinsPopUp(FishInfo info)
    {
        int fishWorth = info.description.baseMonetaryValue.RoundedToInt();
        Vector3 fishPostion = info.transform.position;

        Game.Recolte_UI.GetComponent<SpawnCoinsPopUp>().SpawnPopUp(fishPostion, fishWorth);
    }
}
