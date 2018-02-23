using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float depthRecord;

    public void TryCapture(Capturable fish)
    {
        FishContainer container = Game.SubmarinParts.GetFishContainer();
        if (container.HasRoom())
        {
            fish.Capture();
            container.AddFish(fish.info);

            CallCoinsPopUp(fish.info);
        }
    }

    private void CallCoinsPopUp(FishInfo info)
    {
        int fishWorth = info.description.baseMonetaryValue.RoundedToInt();
        Vector3 fishPostion = info.transform.position;

        Game.Recolte_UI.GetComponent<SpawnCoinsPopUp>().SpawnPopUp(fishPostion, fishWorth);
    }
}
