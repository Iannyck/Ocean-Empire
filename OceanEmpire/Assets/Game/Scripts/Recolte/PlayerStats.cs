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

        if (OnCapture != null)
            OnCapture(fish, captureTechnique);
    }

    private void CallCoinsPopUp(FishInfo info)
    {
        int fishWorth = info.description.baseMonetaryValue.RoundedToInt();
        Vector3 fishPostion = info.transform.position;

        Game.Instance.Recolte_UI.GetComponent<SpawnCoinsPopUp>().SpawnPopUp(fishPostion, fishWorth);
    }
}
