using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FishInfo))]
public class ChestVariableValue : MonoBehaviour
{
    public float baseValue = 25;
    public float mapIndexMultiplier = 20;

    void Start()
    {
        Game.OnGameReady += Game_OnGameReady;
    }

    private void Game_OnGameReady()
    {
        var fishInfo = GetComponent<FishInfo>();
        fishInfo.description = fishInfo.description.Duplicate();
        fishInfo.description.baseMonetaryValue = baseValue + (MapManager.Instance.MapIndex) * mapIndexMultiplier;
    }
}
