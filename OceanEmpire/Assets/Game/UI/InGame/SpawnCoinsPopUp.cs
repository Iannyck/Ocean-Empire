using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoinsPopUp : MonoBehaviour {

    public CoinsPopUp coinsPopUpPrefab;

    public void SpawnPopUp(Vector3 worldPosition, int amount)
    {
        Vector3 screenPostion = Game.GameCamera.cam.GetComponent<Camera>().WorldToScreenPoint(worldPosition);

        CoinsPopUp popUpInstance = Instantiate(coinsPopUpPrefab.gameObject, screenPostion, Quaternion.identity, transform).GetComponent<CoinsPopUp>();
        popUpInstance.SetMoneyAmount(amount);
        popUpInstance.SetWorldPostion(worldPosition);
    }
}
