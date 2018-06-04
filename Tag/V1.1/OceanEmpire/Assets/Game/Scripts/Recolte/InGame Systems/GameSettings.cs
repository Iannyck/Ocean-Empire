using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings
{
    public bool CanUseFishingFrenzy { get; private set; }
    public MapData MapData { get; private set; }

    public GameSettings(MapData mapData, bool canUseFishingFrenzy)
    {
        CanUseFishingFrenzy = canUseFishingFrenzy;
        MapData = mapData;
    }
}
