using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings
{
    public bool CanUseFishingFrenzy { get; private set; }
    public string MapScene { get; private set; }

    public GameSettings(string mapScene, bool canUseFishingFrenzy)
    {
        MapScene = mapScene;
        CanUseFishingFrenzy = canUseFishingFrenzy;
    }
}
