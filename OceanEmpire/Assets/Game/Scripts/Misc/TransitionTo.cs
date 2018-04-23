using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTo : MonoBehaviour {

    public PrebuiltMapData mapData;

    public void GoToScene()
    {
        GameSettings gameSettings = new GameSettings(mapData.MapData, true);

        LoadingScreen.TransitionTo(GameBuilder.SCENENAME, new ToRecolteMessage(gameSettings), true);
    }
}
